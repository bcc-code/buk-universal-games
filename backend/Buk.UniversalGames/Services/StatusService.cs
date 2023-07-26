using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.Record;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Buk.UniversalGames.Services
{
    public class StatusService : IStatusService
    {
        private static readonly int _matchWinnerPoints = 3;

        private readonly ILogger<StatusService> _logger;
        private readonly IStatusRepository _statusRepository;
        private readonly ILeagueRepository _leagueLeagueRepository;
        private readonly DataContext _db;
        private readonly ICacheContext _cache;

        private readonly int[] _scoreByRank = new int[]
        {
            20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1
        };

        private string gameCacheKey(GameType type, int leagueId) => $"ranking_{type}_{leagueId}";
        private string leagueCacheKey(int leagueId) => $"ranking_total_{leagueId}";

        public StatusService(ILogger<StatusService> logger, IStatusRepository statusRepository, ILeagueRepository leagueLeagueRepository, DataContext db, ICacheContext cache)
        {
            _logger = logger;
            _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
            _leagueLeagueRepository = leagueLeagueRepository;
            _db = db;
            _cache = cache;
        }

        public async Task<Dictionary<string, List<TeamStatus>>> GetLeagueRankings(int leagueId)
        {
            var dict = new Dictionary<string, List<TeamStatus>>();
            foreach (var type in (GameType[])Enum.GetValues(typeof(GameType)))
            {
                dict.Add(type.ToString().ToLowerInvariant(), await GetGameRanking(type, leagueId));
            }
            dict.Add("total", await _cache.Get<List<TeamStatus>>(leagueCacheKey(leagueId)) ?? new List<TeamStatus>());

            return dict;
        }

        public async Task<List<TeamStatus>> GetGameRanking(GameType gameType, int leagueId)
        {
            return await _cache.Get<List<TeamStatus>>(gameCacheKey(gameType, leagueId)) ?? new List<TeamStatus>();
        }

        public async Task<List<TeamStatus>> UpdateGameRanking(Game game, int leagueId)
        {
            await BuildAndCacheRankingForGameInLeague(game, leagueId);
            return await BuildAndCacheLeagueRanking(leagueId);
        }

        public async Task<List<TeamStatus>> BuildAndCacheRankingForSidequest(int leagueId)
        {
            var answers = await _cache.Get<string[]>("sidequest_answers");
            if (answers is null || answers.Length == 0) throw new InvalidOperationException("Answers haven't been loaded");

            var teamsWithSize = (from team in _db.Teams
                                 select new { team.TeamId, team.Name, team.MemberCount }).ToList();

            var teamSizeDict = teamsWithSize.ToDictionary(x => x.TeamId, x => x.MemberCount);

            var teamScores = from guess in _db.Guesses
                             where guess.Team.LeagueId == leagueId && answers.Contains(guess.Answer)
                             group guess by new { guess.TeamId, guess.QuestionId } into t
                             select new { t.Key.TeamId, t.Key.QuestionId, Count = t.Count() };

            var relativeScores = teamScores.Select(x => new { x.TeamId, x.QuestionId, x.Count, Percentage = (decimal)x.Count / teamSizeDict[x.TeamId] });
            var averageScores = teamsWithSize
                .Select(t => new
                {
                    t.TeamId,
                    t.Name,
                    AverageScore = relativeScores
                        .Where(x => x.TeamId == t.TeamId)
                        .Average(x => x.Percentage)
                })
                .OrderByDescending(x => x.AverageScore);

            var groupedByRank = averageScores.GroupBy(x => x.AverageScore);

            var rank = -1;
            var ranking = new List<TeamStatus>();
            foreach (var rankingPosition in groupedByRank)
            {
                rank += rankingPosition.Count();
                ranking.AddRange(rankingPosition.Select(x => new TeamStatus(x.TeamId, x.Name, _scoreByRank[rank])));
            }

            await _cache.Set($"sidequest_ranking_{leagueId}", ranking);

            return ranking;
        }

        public async Task<List<TeamStatus>> BuildAndCacheRankingForGameInLeague(Game game, int leagueId)
        {
            var teamScoresQuery = from score in _db.Points
                                  where score.Game == game && score.Match!.LeagueId == leagueId
                                  select new { score.TeamId, score.Team.Name, score.Points };
            var teamScores = await teamScoresQuery.ToListAsync();
            var teamsGroupedByPoints = teamScores.GroupBy(x => x.Points);

            if (game.Type == GameType.TicketTwist || game.Type == GameType.MonkeyBars)
            {
                teamsGroupedByPoints = teamsGroupedByPoints.OrderByDescending(x => x.Key);
            }
            else
            {
                teamsGroupedByPoints = teamsGroupedByPoints.OrderBy(x => x.Key);
            }
            var rank = -1;
            var ranking = new List<TeamStatus>();
            foreach (var rankingPosition in teamsGroupedByPoints)
            {
                rank += rankingPosition.Count();
                ranking.AddRange(rankingPosition.Select(x => new TeamStatus(x.TeamId, x.Name, _scoreByRank[rank]) { Score = x.Points }));
            }

            await _cache.Set(gameCacheKey(game.Type, leagueId), ranking);

            return ranking;
        }

        public async Task<List<TeamStatus>> BuildAndCacheLeagueRanking(int leagueId)
        {
            var teams = await _leagueLeagueRepository.GetTeams(leagueId);

            var matchWinners = from m in _db.Matches
                               where m.LeagueId == leagueId && m.WinnerId.HasValue
                               select m.WinnerId!.Value;

            var nerveSpiral = (await GetGameRanking(GameType.NerveSpiral, leagueId)).ToDictionary(x => x.TeamId);
            var monkeyBars = (await GetGameRanking(GameType.MonkeyBars, leagueId)).ToDictionary(x => x.TeamId);
            var ticketTwist = (await GetGameRanking(GameType.TicketTwist, leagueId)).ToDictionary(x => x.TeamId);
            var minefield = (await GetGameRanking(GameType.MineField, leagueId)).ToDictionary(x => x.TeamId);
            var tableSurfing = (await GetGameRanking(GameType.TableSurfing, leagueId)).ToDictionary(x => x.TeamId);

            var sidequest = (await _cache.Get<List<TeamStatus>>($"sidequest_ranking_{leagueId}"))?.ToDictionary(x => x.TeamId) ?? new Dictionary<int, TeamStatus>();

            var leagueRanking = new List<TeamStatus>();

            foreach (var team in teams)
            {
                var totalPoints = GetPointsForSubRanking(nerveSpiral, team)
                                    + GetPointsForSubRanking(monkeyBars, team)
                                    + GetPointsForSubRanking(ticketTwist, team)
                                    + GetPointsForSubRanking(minefield, team)
                                    + GetPointsForSubRanking(tableSurfing, team)
                                    + GetPointsForSubRanking(sidequest, team) * .7
                                    + _matchWinnerPoints * matchWinners.Count(x => x == team.TeamId);

                leagueRanking.Add(new TeamStatus(team.TeamId, team.Name, (int)(totalPoints * 10)));
            }

            var sortedRanking = leagueRanking.OrderByDescending(x => x.Points).ToList();
            await _cache.Set(leagueCacheKey(leagueId), sortedRanking);
            return sortedRanking;

            static int GetPointsForSubRanking(Dictionary<int, TeamStatus> subRanking, Team team)
                => subRanking.TryGetValue(team.TeamId, out var status) ? status.Points : 0;
        }

        public async Task<TeamStatusReport> GetTeamStatus(Team team)
        {
            if (team is null) throw new ArgumentNullException(nameof(team));

            return new TeamStatusReport
            {
                Status = await _statusRepository.GetTeamStatus(team),
                StatusAt = DateTime.Now
            };
        }

        public async Task<LeagueStatusReport> GetLeagueStatus(int leagueId)
        {
            return new LeagueStatusReport(DateTime.Now, await GetLeagueRankings(leagueId));
        }

        public async Task ClearStatusAndMatches()
        {
            var leagues = await _leagueLeagueRepository.GetLeagues();
            await _statusRepository.ClearStatus(leagues);
        }

        public async Task GuaranteeAnswersInCache()
        {
            var answers = await _db.Settings.FindAsync("answers");
            if (answers is null)
            {
                throw new InvalidOperationException("Did not find the answers to cache");
            }
            await _cache.Set("sidequest_answers", answers.Value);
        }

        public async Task<byte[]> ExportStatus()
        {
            using var stream = new MemoryStream();
            var xlsWorkbook = new XSSFWorkbook();

            var font = xlsWorkbook.CreateFont();
            font.FontHeightInPoints = 11;
            font.FontName = "Calibri";
            font.IsBold = true;

            var style = xlsWorkbook.CreateCellStyle();
            style.SetFont(font);

            var leagues = await _leagueLeagueRepository.GetLeagues();

            foreach (var league in leagues)
            {
                var xlsSheet = xlsWorkbook.CreateSheet(league.Name);

                var rowIndex = 0;
                var row = xlsSheet.CreateRow(rowIndex);

                var cell = row.CreateCell(0);
                cell.CellStyle = style;
                cell.SetCellValue("Position");

                cell = row.CreateCell(1);
                cell.CellStyle = style;
                cell.SetCellValue("Team");

                cell = row.CreateCell(2);
                cell.CellStyle = style;
                cell.SetCellValue("Points");

                cell = row.CreateCell(3);
                cell.CellStyle = style;
                cell.SetCellValue("Stickers");

                var statuses = await _statusRepository.GetLeagueStatus(league.LeagueId);

                rowIndex++;
                foreach (var status in statuses)
                {
                    row = xlsSheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(rowIndex);
                    row.CreateCell(1).SetCellValue(status.Team);
                    row.CreateCell(2).SetCellValue(status.Points);

                    rowIndex++;
                }
            }

            xlsWorkbook.Write(stream);
            return stream.ToArray();
        }
    }
}
