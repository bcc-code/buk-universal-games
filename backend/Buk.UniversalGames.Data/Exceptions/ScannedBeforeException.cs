using Buk.UniversalGames.Data.Models;
using Buk.UniversalGames.Data.Models.Internal;

namespace Buk.UniversalGames.Data.Exceptions
{
    public class ScannedBeforeException : Exception
    {
        public StickerScannedBeforeInfo ScanCheckResult { get; set; }
        public StickerScan Scan { get; set; }

        public ScannedBeforeException(StickerScannedBeforeInfo scanCheckResult, StickerScan scan)
        {
            ScanCheckResult = scanCheckResult;
            Scan = scan;
        }
    }
}
