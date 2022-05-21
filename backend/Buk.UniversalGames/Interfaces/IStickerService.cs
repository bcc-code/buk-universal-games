using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Models;

namespace Buk.UniversalGames.Interfaces
{
    public interface IStickerService
    {
        ScanResult ScanSticker(Team team, string code);
    }
}
