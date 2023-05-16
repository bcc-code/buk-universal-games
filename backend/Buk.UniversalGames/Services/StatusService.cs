using Buk.UniversalGames.Data;
using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Library.Enums;
using Buk.UniversalGames.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using NPOI.HSSF.Record;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Collections.Generic;
using System.ComponentModel;

namespace Buk.UniversalGames.Services
{
    public class StatusService : IStatusService
    {
        private readonly ILogger<StatusService> _logger;
        private readonly IStatusRepository _statusRepository;
        private readonly ILeagueRepository _leagueLeagueRepository;
        private readonly DataContext _db;
        private readonly ICacheContext _cache;

        private readonly GameType[] _gameTypes = (GameType[])Enum.GetValues(typeof(GameType));

        private readonly int[] _scoreByRank = new int[]
        {
            20,19,18,17,16,15,14,13,12,11,10,9,8,7,6,5,4,3,2,1
        };

        public StatusService(ILogger<StatusService> logger, IStatusRepository statusRepository, ILeagueRepository leagueLeagueRepository, DataContext db, ICacheContext cache)
        {
            _logger = logger;
            _statusRepository = statusRepository;
            _leagueLeagueRepository = leagueLeagueRepository;
            _db = db;
            _cache = cache;
        }

        public async Task<Dictionary<string, List<TeamStatus>>> GetLeagueRankings(int leagueId)
        {
            var dict = new Dictionary<string, List<TeamStatus>>();
            foreach(var type in (GameType[])Enum.GetValues(typeof(GameType)))
            {
                dict.Add(type.ToString(), await GetGameRanking(type, leagueId));
            }
            dict.Add("total", await _cache.Get<List<TeamStatus>>($"ranking_total_{leagueId}") ?? new List<TeamStatus>());

            return dict;
        }

        public async Task<List<TeamStatus>> GetGameRanking(GameType gameType, int leagueId)
        {
            return await _cache.Get<List<TeamStatus>>($"ranking_{gameType}_{leagueId}") ?? new List<TeamStatus>();
        }

        public async Task<List<TeamStatus>> UpdateGameRanking(Game game, int leagueId)
        {
            await BuildAndCacheRankingForGameInLeague(game, leagueId);
            return await BuildAndCacheLeagueRanking(leagueId);
        }

        public async Task<List<TeamStatus>> BuildAndCacheRankingForGameInLeague(Game game, int leagueId)
        {
            var teamScores = from score in _db.Points
                             join match in _db.Matches on score.Match equals match
                             join league in _db.Leagues on match.League equals league
                             join team in _db.Teams on score.Team equals team
                             where score.Game == game && league.LeagueId == leagueId
                             select new { score.TeamId, team.Name, score.Points };
            var teamScoresQuery = (await teamScores.ToListAsync()).GroupBy(x => x.Points);

            if (game.Type == GameType.TicketTwist || game.Type == GameType.MonkeyBars)
            {
                teamScoresQuery = teamScoresQuery.OrderByDescending(x => x.Key);
            }
            else
            {
                teamScoresQuery = teamScoresQuery.OrderBy(x => x.Key);
            }
            var rank = -1;
            var ranking = new List<TeamStatus>();
            foreach (var rankingPosition in teamScoresQuery)
            {
                rank += rankingPosition.Count();
                ranking.AddRange(rankingPosition.Select(x => new TeamStatus(x.TeamId, x.Name, _scoreByRank[rank]) { Score = x.Points }));
            }

            await _cache.Set($"ranking_{game.GameType}_{leagueId}", ranking);

            return ranking;
        }

        public async Task<List<TeamStatus>> BuildAndCacheLeagueRanking(int leagueId)
        {
            var teams = await _leagueLeagueRepository.GetTeams(leagueId);

            var nerveSpiral = (await GetGameRanking(GameType.NerveSpiral, leagueId)).ToDictionary(x => x.TeamId);
            var monkeyBars = (await GetGameRanking(GameType.MonkeyBars, leagueId)).ToDictionary(x => x.TeamId);
            var ticketTwist = (await GetGameRanking(GameType.TicketTwist, leagueId)).ToDictionary(x => x.TeamId);
            var minefield = (await GetGameRanking(GameType.MineField, leagueId)).ToDictionary(x => x.TeamId);
            var tableSurfing = (await GetGameRanking(GameType.TableSurfing, leagueId)).ToDictionary(x => x.TeamId);

            var leagueRanking = new List<TeamStatus>();

            foreach(var team in teams)
            {
                var totalPoints = GetPointsForGame(nerveSpiral, team) 
                                    + GetPointsForGame(monkeyBars, team)
                                    + GetPointsForGame(ticketTwist, team)
                                    + GetPointsForGame(minefield, team)
                                    + GetPointsForGame(tableSurfing, team);
                leagueRanking.Add(new TeamStatus(team.TeamId, team.Name, totalPoints));
            }

            await _cache.Set($"ranking_total_{leagueId}", leagueRanking);

            return leagueRanking;

            static int GetPointsForGame(Dictionary<int, TeamStatus> nerveSpiral, Team team) 
                => nerveSpiral.TryGetValue(team.TeamId, out var status) ? status.Points : 0;
        }

        public async Task<TeamStatusReport> GetTeamStatus(Team team)
        {
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
