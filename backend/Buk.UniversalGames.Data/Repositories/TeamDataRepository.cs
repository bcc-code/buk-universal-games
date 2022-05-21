using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Repositories
{
    public class TeamDataRepository : ITeamRepository
    {
        private readonly DataContext _db;

        public TeamDataRepository(DataContext db)
        {
            _db = db;
        }

        public Team? GetTeamByCode(string code)
        {
            return null;
            //return _db.Teams.FirstOrDefault(t=>t.Code == code);
        }
    }
}
