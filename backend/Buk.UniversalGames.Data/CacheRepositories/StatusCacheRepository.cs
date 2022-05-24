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

        public StatusCacheRepository(ILogger<StatusCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _data = new StatusDataRepository(dataContext);
        }

        public TeamStatus GetTeamStatus(Team team)
        {
            return _data.GetTeamStatus(team);
        }

        public List<TeamStatus> GetLeagueStatus(int leagueId)
        {
            return _data.GetLeagueStatus(leagueId);
        }
    }
}
