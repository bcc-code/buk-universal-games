using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class StatusCacheRepository : IStatusRepository
    {
        private readonly ILogger<StatusCacheRepository> _logger;
        private readonly StatusDataRepository _data;
        private readonly ICacheContext _cache;

        public StatusCacheRepository(ILogger<StatusCacheRepository> logger, DataContext dataContext, ICacheContext cache)
        {
            _logger = logger;
            _data = new StatusDataRepository(dataContext);
            _cache = cache;
        }

        public TeamStatus? GetTeamStatus(Team team)
        {
            // we want league status always calculated and set in cache, so get league status
            var leagueStatuses = GetLeagueStatus(team.LeagueId.GetValueOrDefault());

            // find team status
            var status = leagueStatuses.FirstOrDefault(s=>s.TeamId == team.TeamId);

            // fallback to calculation
            return status ?? _data.GetTeamStatus(team);
        }

        public List<TeamStatus> GetLeagueStatus(int leagueId)
        {
            // get from cache
            var cacheKey = $"LeagueStatus_{leagueId}";
            var leagueStatus = _cache.Get<List<TeamStatus>>(cacheKey);
            if (leagueStatus == null)
            {
                // fallback to db and set in cache
                leagueStatus = _data.GetLeagueStatus(leagueId); 
                _cache.Set(cacheKey, leagueStatus);
            }
            return leagueStatus;
        }

        public void ClearStatus(List<League> leagues)
        {
            _data.ClearStatus(leagues);

            foreach (var league in leagues)
            {
                // clear match lists for league and league status
                _cache.Remove($"Matches_{league.LeagueId}");
                _cache.Remove($"LeagueStatus_{league.LeagueId}");
            }
        }
    }
}
