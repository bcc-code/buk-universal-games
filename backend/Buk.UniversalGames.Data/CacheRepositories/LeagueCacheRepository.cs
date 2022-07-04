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

        public League? GetLeague(int leagueId)
        {
            var leagues = GetLeagues();
            var league = leagues.FirstOrDefault(s => s.LeagueId == leagueId);
            return league ?? _data.GetLeague(leagueId);
        }

        public List<League> GetLeagues()
        {
            // get from cache
            var cacheKey = "Leagues";
            var result = _cache.Get<List<League>>(cacheKey);
            if (result == null)
            {
                // fallback to db and set in cache
                result = _data.GetLeagues();
                _cache.Set(cacheKey, result);
            }
            return result;
        }

        public Team? GetTeam(int teamId)
        {
            var cacheKey = $"TeamId_{teamId}";
            var team = _cache.Get<Team>(cacheKey);
            if (team == null)
            {
                team = _data.GetTeam(teamId);
                if (team != null)
                    _cache.Set(cacheKey, team);
            }
            return team;
        }

        public Team? GetTeamByCode(string code)
        {
            var cacheKey = $"Team_{code}";
            var team = _cache.Get<Team>(cacheKey);
            if (team == null)
            {
                team = _data.GetTeamByCode(code);
                if (team != null)
                    _cache.Set(cacheKey, team);
            }
            return team;
        }

        public List<Team> GetTeams(int leagueId)
        {
            // get from cache
            var cacheKey = $"Teams_{leagueId}";
            var teams = _cache.Get<List<Team>>(cacheKey);
            if (teams == null)
            {
                // fallback to db and set in cache
                teams = _data.GetTeams(leagueId);
                _cache.Set(cacheKey, teams);

                foreach (var team in teams)
                {
                    _cache.Set($"TeamId_{team.TeamId}", team);
                    _cache.Set($"Team_{team.Code}", team);
                }
            }
            return teams;
        }
    }
}
