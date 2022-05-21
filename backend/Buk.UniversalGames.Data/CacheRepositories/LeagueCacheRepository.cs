using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class LeagueCacheRepository : ILeagueRepository
    {
        private readonly ILogger<LeagueCacheRepository> _logger;
        private readonly LeagueDataRepository _leagueRepository;

        public LeagueCacheRepository(ILogger<LeagueCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _leagueRepository = new LeagueDataRepository(dataContext);
        }

        public IEnumerable<League> GetLeagues()
        {
            return _leagueRepository.GetLeagues();
        }

        public IEnumerable<Team> GetTeams(int leagueId)
        {
            return _leagueRepository.GetTeams(leagueId);
        }
    }
}
