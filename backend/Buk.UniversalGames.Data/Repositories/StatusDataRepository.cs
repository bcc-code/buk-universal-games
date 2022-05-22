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
    }
}
