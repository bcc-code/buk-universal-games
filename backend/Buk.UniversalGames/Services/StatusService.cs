using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Interfaces;
using Buk.UniversalGames.Models;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Services
{
    public class StatusService : IStatusService
    {
        private readonly ILogger<StatusService> _logger;
        private readonly IStatusRepository _statusRepository;
        private readonly ILeagueRepository _leagueLeagueRepository;

        public StatusService(ILogger<StatusService> logger, IStatusRepository statusRepository, ILeagueRepository leagueLeagueRepository)
        {
            _logger = logger;
            _statusRepository = statusRepository;
            _leagueLeagueRepository = leagueLeagueRepository;
        }

        public TeamStatusReport GetTeamStatus(Team team)
        {
            return new TeamStatusReport
            {
                Status = _statusRepository.GetTeamStatus(team),
                StatusAt = DateTime.Now
            };
        }

        public LeagueStatusReport GetLeagueStatus(int leagueId)
        {
            return new LeagueStatusReport
            {
                Status = _statusRepository.GetLeagueStatus(leagueId),
                StatusAt = DateTime.Now
            };
        }

        public void ClearStatusAndMatches()
        {
            var leagues = _leagueLeagueRepository.GetLeagues();
            _statusRepository.ClearStatus(leagues);
        }
    }
}
