using Buk.UniversalGames.Data.Interfaces;

namespace Buk.UniversalGames.Data.Repositories
{
    public class MatchDataRepository : IMatchRepository
    {
        private readonly DataContext _db;

        public MatchDataRepository(DataContext db)
        {
            _db = db;
        }

       
    }
}
