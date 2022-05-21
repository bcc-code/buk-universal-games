using Buk.UniversalGames.Data.Interfaces;

namespace Buk.UniversalGames.Data.Repositories
{
    public class ScoreDataRepository : IScoreRepository
    {
        private readonly DataContext _db;

        public ScoreDataRepository(DataContext db)
        {
            _db = db;
        }
    }
}
