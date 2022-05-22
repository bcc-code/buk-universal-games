using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Repositories
{
    public class StatusDataRepository : IStatusRepository
    {
        private readonly DataContext _db;

        public StatusDataRepository(DataContext db)
        {
            _db = db;
        }

        public List<Point> GetTeamPoints(Team team)
        {
            return _db.Points.Where(s => s.TeamId == team.TeamId).ToList();
        }

        public List<Point> GetLeaguePoints(int leagueId)
        {
            return (from team in _db.Teams.Where(s => s.LeagueId == leagueId)
                from point in _db.Points.Where(s => s.TeamId == team.TeamId)
                select point).ToList();
        }
    }
}
