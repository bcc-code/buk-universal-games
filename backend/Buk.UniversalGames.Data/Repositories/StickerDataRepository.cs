using Buk.UniversalGames.Data.Interfaces;

namespace Buk.UniversalGames.Data.Repositories
{
    public class StickerDataRepository : IStickerRepository
    {
        private readonly DataContext _db;

        public StickerDataRepository(DataContext db)
        {
            _db = db;
        }

       
    }
}
