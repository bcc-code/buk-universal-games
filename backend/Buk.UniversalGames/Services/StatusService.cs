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
            foreach(var type in (GameType[])Enum.GetValues(typeof(GameType)))
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
            var teamSizes = (from team in _db.Teams
                             select new { team.TeamId, team.Name, team.MemberCount }).ToList();
            
            var teamSizeDict = teamSizes.ToDictionary(x => x.TeamId, x => x.MemberCount);

            var teamScores = from answer in _db.Guesses
                             where answer.Team.LeagueId == leagueId && answer.IsCorrect
                             group answer by new { answer.TeamId, answer.QuestionId } into t
                             select new { t.Key.TeamId, t.Key.QuestionId, Count = t.Count() };

            var relativeScores = teamScores.Select(x => new { x.TeamId, x.QuestionId, x.Count, Percentage = (decimal)x.Count / teamSizeDict[x.TeamId] });
            var averageScores = teamSizes
                .Select(t => new { 
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

            await _cache.Set("sidequest_ranking", ranking);

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
            
            var sidequest = (await _cache.Get<List<TeamStatus>>("sidequest_ranking"))?.ToDictionary(x => x.TeamId) ?? new Dictionary<int, TeamStatus>();

            var leagueRanking = new List<TeamStatus>();

            foreach(var team in teams)
            {
                var totalPoints = GetPointsForSubRanking(nerveSpiral, team) 
                                    + GetPointsForSubRanking(monkeyBars, team)
                                    + GetPointsForSubRanking(ticketTwist, team)
                                    + GetPointsForSubRanking(minefield, team)
                                    + GetPointsForSubRanking(tableSurfing, team)
                                    + GetPointsForSubRanking(sidequest, team)
                                    + _matchWinnerPoints * matchWinners.Count(x => x == team.TeamId);

                leagueRanking.Add(new TeamStatus(team.TeamId, team.Name, totalPoints));
            }

            var sortedRanking = leagueRanking.OrderByDescending(x => x.Points).ToList();
            await _cache.Set(leagueCacheKey(leagueId), sortedRanking);
            return sortedRanking;

            static int GetPointsForSubRanking (Dictionary<int, TeamStatus> subRanking, Team team) 
                => subRanking.TryGetValue(team.TeamId, out var status) ? status.Points : 0;
        }

        public async Task<TeamStatusReport> GetTeamStatus(Team team)
        {
            if(team is null) throw new ArgumentNullException(nameof(team));

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
    }
}
