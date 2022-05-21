using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly ILogger<LeagueService> _logger;
        private readonly ILeagueRepository _leagueRepository;

        public LeagueService(ILogger<LeagueService> logger, ILeagueRepository leagueRepository)
        {
            _logger = logger;
            _leagueRepository = leagueRepository;
        }

        public League? GetLeague(int leagueId)
        {
            return _leagueRepository.GetLeague(leagueId);
        }

        public List<League> GetLeagues()
        {
            return _leagueRepository.GetLeagues();
        }

        public Team? GetTeamByCode(string code)
        {
            return _leagueRepository.GetTeamByCode(code);
        }

        public List<Team> GetTeams(int leagueId)
        {
            return _leagueRepository.GetTeams(leagueId);
        }
    }
}
