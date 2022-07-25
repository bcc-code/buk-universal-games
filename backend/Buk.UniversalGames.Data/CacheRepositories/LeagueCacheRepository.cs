using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class LeagueCacheRepository : ILeagueRepository
    {
        private readonly ILogger<LeagueCacheRepository> _logger;
        private readonly LeagueDataRepository _data;
        private readonly ICacheContext _cache;

        public LeagueCacheRepository(ILogger<LeagueCacheRepository> logger, DataContext dataContext, ICacheContext cache)
        {
            _logger = logger;
            _data = new LeagueDataRepository(dataContext);
            _cache = cache;
        }

        public async Task<League?> GetLeague(int leagueId)
        {
            var leagues = await GetLeagues();
            var league = leagues.FirstOrDefault(s => s.LeagueId == leagueId);
            return league ?? await _data.GetLeague(leagueId);
        }

        public async Task<List<League>> GetLeagues()
        {
            // get from cache
            var cacheKey = "Leagues";
            var result = await _cache.Get<List<League>>(cacheKey);
            if (result == null)
            {
                // fallback to db and set in cache
                result = await _data.GetLeagues();
                await _cache.Set(cacheKey, result);
            }
            return result;
        }

        public async Task<Team?> GetTeam(int teamId)
        {
            var cacheKey = $"TeamId_{teamId}";
            var team = await _cache.Get<Team>(cacheKey);
            if (team == null)
            {
                team = await _data.GetTeam(teamId);
                if (team != null)
                    await _cache.Set(cacheKey, team);
            }
            return team;
        }

        public async Task<Team?> GetTeamByCode(string code)
        {
            var cacheKey = $"Team_{code}";
            var team = await _cache.Get<Team>(cacheKey);
            if (team == null)
            {
                team = await _data.GetTeamByCode(code);
                if (team != null)
                    await _cache.Set(cacheKey, team);
            }
            return team;
        }

        public async Task<List<Team>> GetTeams(int leagueId)
        {
            // get from cache
            var cacheKey = $"Teams_{leagueId}";
            var teams = await _cache.Get<List<Team>>(cacheKey);
            if (teams == null)
            {
                // fallback to db and set in cache
                teams = await _data.GetTeams(leagueId);
                await _cache.Set(cacheKey, teams);

                foreach (var team in teams)
                {
                    await _cache.Set($"TeamId_{team.TeamId}", team);
                    await _cache.Set($"Team_{team.Code}", team);
                }
            }
            return teams;
        }
    }
}
