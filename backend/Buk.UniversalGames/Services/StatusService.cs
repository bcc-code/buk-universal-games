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

        public StatusService(ILogger<StatusService> logger, IStatusRepository statusRepository)
        {
            _logger = logger;
            _statusRepository = statusRepository;
        }

        public TeamStatus GetTeamStatus(Team team)
        {
            var points = _statusRepository.GetTeamPoints(team);
            return new TeamStatus
            {
                TeamId = team.TeamId,
                Points = points.Sum(s => s.Points),
                Stickers = points.Count(s => s.StickerId.GetValueOrDefault() > 0),
                StatusAt = DateTime.Now
            };
        }

        public List<TeamStatus> GetLeagueStatus(int leagueId)
        {
            var points = _statusRepository.GetLeaguePoints(leagueId);

            return points.GroupBy(s => new
                {
                    s.TeamId,
                })
                .Select(s => new TeamStatus
                {
                    TeamId = s.Key.TeamId,
                    Points = s.Sum(s => s.Points),
                    Stickers = s.Count(s => s.StickerId.GetValueOrDefault() > 0),
                    StatusAt = DateTime.Now
                })
                .OrderByDescending(s => s.Points)
                .ToList();
        }
    }
}
