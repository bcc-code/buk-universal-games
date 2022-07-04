using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Interfaces
{
    public interface IStickerRepository
    {
        Sticker? GetStickerByCode(string code);

        StickerScanResult LogStickerScanning(Team team, Sticker sticker);

        List<Sticker> GetStickers(int leagueId);

        void SetRandomStickerPoints();
    }
}
