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

        public async Task<Sticker?> GetStickerByCode(string code)
        {
            var cacheKey = $"Sticker_{code}";
            var sticker = await _cache.Get<Sticker>(cacheKey);
            if (sticker == null)
            {
                sticker = await _data.GetStickerByCode(code);
                await _cache.Set(cacheKey, sticker);
            }
            return sticker;
        }

        public async Task<StickerScanResult> LogStickerScanning(Team team, Sticker sticker)
        {
            var result = await _data.LogStickerScanning(team, sticker);
            await _cache.Remove($"LeagueStatus_{team.LeagueId.GetValueOrDefault()}");
            return result;
        }

        public async Task<List<Sticker>> GetStickers(int leagueId)
        {
            var cacheKey = $"Stickers_{leagueId}";
            var stickers = await _cache.Get<List<Sticker>>(cacheKey);
            if (stickers == null)
            {
                stickers = await _data.GetStickers(leagueId);
                await _cache.Set(cacheKey, stickers);

                foreach (var sticker in stickers)
                    await _cache.Set($"Sticker_{sticker.Code}", sticker);
            }
            return await _data.GetStickers(leagueId);
        }

        public async Task SetRandomStickerPoints()
        {
            await _data.SetRandomStickerPoints();
        }
    }
}
