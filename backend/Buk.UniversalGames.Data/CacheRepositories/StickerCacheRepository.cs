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

        public StickerCacheRepository(ILogger<StickerCacheRepository> logger, DataContext dataContext)
        {
            _logger = logger;
            _data = new StickerDataRepository(dataContext);
        }

        public Sticker? GetStickerByCode(string code)
        {
            return _data.GetStickerByCode(code);
        }

        public StickerScanResult LogStickerScanning(Team team, Sticker sticker)
        {
            return _data.LogStickerScanning(team, sticker);
        }

        public Sticker? GetSticker(int stickerId)
        {
            return _data.GetSticker(stickerId);
        }

        public Sticker? GetSticker(string stickerCode)
        {
            return _data.GetSticker(stickerCode);
        }

        public List<Sticker> GetStickers(int leagueId)
        {
            return _data.GetStickers(leagueId);
        }
    }
}
