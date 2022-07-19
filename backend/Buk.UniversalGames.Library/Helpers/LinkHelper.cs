namespace Buk.UniversalGames.Library.Helpers
{
    public class LinkHelper
    {
        public const string AppLink = "https://universalgames.buk.no";
        public static string GetStickerLink(string stickerCode)
        {
            return $"{AppLink}/#/scan/{stickerCode}";
        }

        public static string GetStickerQRLInk(string stickerCode)
        {
            return $"{AppLink}/api/QR/{stickerCode}";
        }
    }
}
