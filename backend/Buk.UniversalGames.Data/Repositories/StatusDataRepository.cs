using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Repositories
{
    public class StatusDataRepository : IStatusRepository
    {
        private readonly DataContext _db;

        public StatusDataRepository(DataContext db)
        {
            _db = db;
        }

        public TeamStatus? GetTeamStatus(Team team)
        {
            var points = _db.Points.Where(p => p.TeamId == team.TeamId).ToList();
            return new TeamStatus
            {
                Team = team.Name,
                TeamId = team.TeamId,
                Points = points.Sum(s=>s.Points),
                Stickers = points.Count(s=>s.StickerId != null)
            };
        }

        public List<TeamStatus> GetLeagueStatus(int leagueId)
        {
            return (from team in _db.Teams
                    where team.LeagueId == leagueId
                    join point in _db.Points on team.TeamId equals point.TeamId into teamPoints
                    from teamPoint in teamPoints.DefaultIfEmpty()
                    group teamPoint by new { team.TeamId, team.Name }
                    into grouped
                    select new TeamStatus
                    {
                        TeamId = grouped.Key.TeamId,
                        Team = grouped.Key.Name,
                        Points = grouped.Sum(s => s.Points),
                        Stickers = grouped.Count(s => s.StickerId != null)
                    }
                ).OrderByDescending(s=>s.Points).ThenBy(s=>s.Team).ToList();
        }
    }
}
