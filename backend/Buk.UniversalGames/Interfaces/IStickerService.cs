using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStickerService
    {
        Task<ScanResult> ScanSticker(Team team, string code);

        Task<List<Sticker>> GetStickers(int leagueId);

        Task<byte[]?> GetStickerQR(string stickerCode, int size = 40);

        Task<byte[]> ExportStickers();

        Task SetRandomStickerPoints();
    }
}
