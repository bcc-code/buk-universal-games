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

        public TeamStatus GetTeamStatus(Team team)
        {
            return _data.GetTeamStatus(team);
        }

        public List<TeamStatus> GetLeagueStatus(int leagueId)
        {
            var cacheKey = $"LeagueStatus_{leagueId}";
            var leagueStatus = _cache.Get<List<TeamStatus>>(cacheKey);

            if (leagueStatus == null)
            {
                leagueStatus = _data.GetLeagueStatus(leagueId); 
                _cache.Set(cacheKey, leagueStatus);
            }
            return leagueStatus;
        }
    }
}
