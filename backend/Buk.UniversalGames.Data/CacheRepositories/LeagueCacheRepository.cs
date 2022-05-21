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

        public LeagueCacheRepository(ILogger<LeagueCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _data = new LeagueDataRepository(dataContext);
        }

        public League? GetLeague(int leagueId)
        {
            return _data.GetLeague(leagueId);
        }

        public List<League> GetLeagues()
        {
            return _data.GetLeagues();
        }

        public Team? GetTeamByCode(string code)
        {
            return _data.GetTeamByCode(code);
        }

        public List<Team> GetTeams(int leagueId)
        {
            return _data.GetTeams(leagueId);
        }
    }
}
