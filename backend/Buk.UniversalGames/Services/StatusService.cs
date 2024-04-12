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
using NPOI.HSSF.Record.Chart;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Math.EC.Rfc7748;
using StackExchange.Redis;
using System.Linq.Expressions;

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
            20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1,0,0,0,0
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
            var answerString = await _cache.Get<string>("sidequest_answers");
            if (answerString is null || answerString.Length == 0) throw new InvalidOperationException("Answers haven't been loaded");

            var answers = answerString.Split(',');

            var teamsWithSize = (from team in _db.Teams
                                 where team.TeamType == "participant"
                                 select new { team.TeamId, team.Name, team.MemberCount }).ToList();

            var teamSizeDict = teamsWithSize.ToDictionary(x => x.TeamId, x => x.MemberCount);

            var teamScores = from guess in _db.Guesses
                             where guess.Team.LeagueId == leagueId && answers.Contains(guess.Answer)
                             group guess by new { guess.TeamId, guess.QuestionId } into t
                             select new { t.Key.TeamId, t.Key.QuestionId, Count = t.Count() };

            var relativeScores = (await teamScores.ToListAsync()).Select(x => new { x.TeamId, x.QuestionId, x.Count, Percentage = (teamSizeDict[x.TeamId] == 0 || x.Count > Math.Ceiling(teamSizeDict[x.TeamId] * 1.10)) ? 0 : (decimal)x.Count / teamSizeDict[x.TeamId] });
            var sumScore = teamsWithSize
                .Select(t => new
                {
                    t.TeamId,
                    t.Name,
                    ScoreSum = relativeScores
                        .Where(x => x.TeamId == t.TeamId)
                        .Sum(x => x.Percentage)
                })
                .Where(x => x.ScoreSum > 0)
                .OrderByDescending(x => x.ScoreSum);

            var groupedByRank = sumScore.GroupBy(x => x.ScoreSum);

            var rank = -1;
            var ranking = new List<TeamStatus>();
            foreach (var rankingPosition in groupedByRank)
            {
                rank += rankingPosition.Count();
                ranking.AddRange(rankingPosition.Select(x => new TeamStatus(x.TeamId, x.Name, rank < 0 ? 0 : _scoreByRank[rank])));
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

            if (game.Type == GameType.Labyrinth || game.Type == GameType.HumanShuffleBoard)
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

            var matchWinners = await (from m in _db.Matches
                                      where m.LeagueId == leagueId && m.WinnerId.HasValue
                                      select m.WinnerId!.Value).ToListAsync();

            // Fetching rankings
            var landWaterBeachRankings = await GetGameRanking(GameType.LandWaterBeach, leagueId);
            var humanShuffleBoardRankings = await GetGameRanking(GameType.HumanShuffleBoard, leagueId);
            var labyrinthRankings = await GetGameRanking(GameType.Labyrinth, leagueId);
            var mastermindRankings = await GetGameRanking(GameType.Mastermind, leagueId);
            var ironGripRankings = await GetGameRanking(GameType.IronGrip, leagueId);

            // Formatting the string
            var errorMessage = $"LandWaterBeach: {FormatRankings(landWaterBeachRankings)}, " +
                               $"HumanShuffleBoard: {FormatRankings(humanShuffleBoardRankings)}, " +
                               $"Labyrinth: {FormatRankings(labyrinthRankings)}, " +
                               $"Mastermind: {FormatRankings(mastermindRankings)}, " +
                               $"IronGrip: {FormatRankings(ironGripRankings)}";

            // Throw the formatted string as part of an exception
            throw new Exception(errorMessage);

            // Proceed with converting to dictionaries and calculating points
            var landWaterBeach = landWaterBeachRankings.ToDictionary(x => x.TeamId);
            var humanShuffleBoard = humanShuffleBoardRankings.ToDictionary(x => x.TeamId);
            var labyrinth = labyrinthRankings.ToDictionary(x => x.TeamId);
            var mastermind = mastermindRankings.ToDictionary(x => x.TeamId);
            var ironGrip = ironGripRankings.ToDictionary(x => x.TeamId);

            var sidequest = (await _cache.Get<List<TeamStatus>>($"sidequest_ranking_{leagueId}"))?.ToDictionary(x => x.TeamId) ?? new Dictionary<int, TeamStatus>();

            var leagueRanking = new List<TeamStatus>();

            foreach (var team in teams)
            {
                var totalPoints = GetPointsForSubRanking(landWaterBeach, team)
                                    + GetPointsForSubRanking(humanShuffleBoard, team)
                                    + GetPointsForSubRanking(labyrinth, team)
                                    + GetPointsForSubRanking(mastermind, team)
                                    + GetPointsForSubRanking(ironGrip, team)
                                    + (GetPointsForSubRanking(sidequest, team) * .7M)
                                    + (_matchWinnerPoints * matchWinners.Count(x => x == team.TeamId));

                leagueRanking.Add(new TeamStatus(team.TeamId, team.Name, (int)(totalPoints * 10)));
            }

            var sortedRanking = leagueRanking.OrderByDescending(x => x.Points).ToList();
            await _cache.Set(leagueCacheKey(leagueId), sortedRanking);
            return sortedRanking;

            static int GetPointsForSubRanking(Dictionary<int, TeamStatus> subRanking, Team team)
                => subRanking.TryGetValue(team.TeamId, out var status) ? status.Points : 0;
        }

        private string FormatRankings(List<TeamStatus> rankings)
        {
            return string.Join(", ", rankings.Select(x => $"TeamId: {x.TeamId}, Team: {x.Team}, Points: {x.Points}, Score: {x.Score}"));
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
            var answers = await _db.Settings.FindAsync("answers") ?? throw new InvalidOperationException("Did not find the answers to cache");
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

            var boldCellStyle = xlsWorkbook.CreateCellStyle();
            boldCellStyle.SetFont(font);

            var leagues = await _leagueLeagueRepository.GetLeagues();

            var mainSheet = xlsWorkbook.CreateSheet("Winners");

            var titleRow = mainSheet.CreateRow(0);
            Cell(titleRow, 3, "B-League", boldCellStyle);
            Cell(titleRow, 5, "U-League", boldCellStyle);
            Cell(titleRow, 7, "K-League", boldCellStyle);

            var overallWinnerRow = mainSheet.CreateRow(1);
            Cell(overallWinnerRow, 1, "Overall winner", boldCellStyle);

            var games = (GameType[])Enum.GetValues(typeof(GameType));

            var gameRowIndex = (GameType game) => game switch
            {
                GameType.LandWaterBeach => 5,
                GameType.IronGrip => 6,
                GameType.Mastermind => 7,
                GameType.Labyrinth => 8,
                GameType.HumanShuffleBoard => 9,
                _ => throw new Exception()
            };

            var gameColumnIndex = (GameType game) => game switch
            {
                GameType.LandWaterBeach => 3,
                GameType.IronGrip => 5,
                GameType.Mastermind => 7,
                GameType.Labyrinth => 9,
                GameType.HumanShuffleBoard => 11,
                _ => throw new Exception()
            };

            var gameRows = games.ToDictionary(x => x, x => mainSheet.CreateRow(gameRowIndex(x)));

            foreach (var league in leagues)
            {
                var status = await GetLeagueRankings(league.LeagueId);

                if (!status["total"].Any()) continue;

                var leagueCol = league.Name switch
                {
                    "B-League" => 3,
                    "U-League" => 5,
                    "K-League" => 7,
                    _ => 8
                };

                var leagueSheet = xlsWorkbook.CreateSheet(league.Name);
                var leagueTitleRow = leagueSheet.CreateRow(0);

                //fill league winner in main sheet
                Cell(overallWinnerRow, leagueCol, status["total"].First().Team);
                Cell(overallWinnerRow, leagueCol + 1, status["total"].First().Points);

                //overall ranking
                Cell(leagueTitleRow, 1, "Overall");

                var rowIndex = 2;
                foreach (var team in status["total"])
                {
                    var row = leagueSheet.CreateRow(rowIndex);
                    Cell(row, 1, team.Team);
                    Cell(row, 2, team.Points);
                    rowIndex++;
                }

                foreach (var game in games)
                {
                    var gameRanking = status[game.ToString().ToLowerInvariant()];

                    if (!gameRanking.Any()) continue;

                    // fill game winner on main sheet
                    Cell(gameRows[game], 1, game.ToString());
                    Cell(gameRows[game], leagueCol, gameRanking.First().Team);
                    Cell(gameRows[game], leagueCol + 1, gameRanking.First().Points);

                    // handle league sheet
                    Cell(leagueTitleRow, gameColumnIndex(game), game.ToString(), boldCellStyle);
                    Cell(leagueTitleRow, gameColumnIndex(game) + 1, $"Score ({game})", boldCellStyle);

                    rowIndex = 2;
                    foreach (var team in gameRanking)
                    {
                        Cell(leagueSheet.GetRow(rowIndex), gameColumnIndex(game), team.Team);
                        Cell(leagueSheet.GetRow(rowIndex), gameColumnIndex(game) + 1, team.Points);
                        rowIndex++;
                    }
                }
            }
            xlsWorkbook.Write(stream);
            return stream.ToArray();
        }

        private static ICell Cell(IRow row, int index, string value, ICellStyle? cellStyle = null)
        {
            var cell = row.CreateCell(index);
            if (cellStyle != null)
                cell.CellStyle = cellStyle;

            cell.SetCellValue(value);
            return cell;
        }

        private static ICell Cell(IRow row, int index, int value, ICellStyle? cellStyle = null)
        {
            var cell = row.CreateCell(index);
            if (cellStyle != null)
                cell.CellStyle = cellStyle;

            cell.SetCellValue(value);
            return cell;
        }
    }
}
