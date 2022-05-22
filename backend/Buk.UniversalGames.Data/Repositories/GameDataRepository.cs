using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;

namespace Buk.UniversalGames.Data.Repositories
{
    public class GameDataRepository : IGameRepository
    {
        private readonly DataContext _db;

        public GameDataRepository(DataContext db)
        {
            _db = db;
        }

        public List<Game> GetGames()
        {
            return _db.Games.ToList();
        }
    }
}
