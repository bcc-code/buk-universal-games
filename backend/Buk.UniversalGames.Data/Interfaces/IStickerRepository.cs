using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IStickerRepository
    {
        Task<Sticker?> GetStickerByCode(string code);

        Task<StickerScanResult> LogStickerScanning(Team team, Sticker sticker);

        Task<List<Sticker>> GetStickers(int leagueId);

        Task SetRandomStickerPoints();
    }
}
