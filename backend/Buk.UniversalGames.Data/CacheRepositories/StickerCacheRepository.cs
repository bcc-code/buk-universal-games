using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class StickerCacheRepository : IStickerRepository
    {
        private readonly ILogger<StickerCacheRepository> _logger;
        private readonly StickerDataRepository _data;

        public StickerCacheRepository(ILogger<StickerCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _data = new StickerDataRepository(dataContext);
        }
    }
}
