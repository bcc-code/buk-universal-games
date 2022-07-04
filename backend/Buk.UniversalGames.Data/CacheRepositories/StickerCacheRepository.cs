using Buk.UniversalGames.Data.Interfaces;
using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;
using Buk.UniversalGames.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Buk.UniversalGames.Data.CacheRepositories
{
    public class StickerCacheRepository : IStickerRepository
    {
        private readonly ILogger<StickerCacheRepository> _logger;
        private readonly StickerDataRepository _data;
        private readonly ICacheContext _cache;

        public StickerCacheRepository(ILogger<StickerCacheRepository> logger, DataContext dataContext, ICacheContext cache)
        {
            _logger = logger;
            _data = new StickerDataRepository(dataContext);
            _cache = cache;
        }

        public Sticker? GetStickerByCode(string code)
        {
            var cacheKey = $"Sticker_{code}";
            var sticker = _cache.Get<Sticker>(cacheKey);
            if (sticker == null)
            {
                sticker = _data.GetStickerByCode(code);
                _cache.Set(cacheKey, sticker);
            }
            return sticker;
        }

        public StickerScanResult LogStickerScanning(Team team, Sticker sticker)
        {
            var result = _data.LogStickerScanning(team, sticker);

            _cache.Remove($"LeagueStatus_{team.LeagueId.GetValueOrDefault()}");

            return result;
        }

        public List<Sticker> GetStickers(int leagueId)
        {
            var cacheKey = $"Stickers_{leagueId}";
            var stickers = _cache.Get<List<Sticker>>(cacheKey);
            if (stickers == null)
            {
                stickers = _data.GetStickers(leagueId);
                _cache.Set(cacheKey, stickers);

                foreach (var sticker in stickers)
                {
                    _cache.Set($"Sticker_{sticker.Code}", sticker);
                }
            }
            return _data.GetStickers(leagueId);
        }

        public void SetRandomStickerPoints()
        {
            _data.SetRandomStickerPoints();
        }
    }
}
