namespace Buk.UniversalGames.Library.Helpers
{
    public class StickerHelper
    {
        public static byte[]? GetQRImage(string stickerCode, string color,  int size = 40)
        {
            // get qr link
            var link = LinkHelper.GetStickerLink(stickerCode);
            return QRHelper.GetQRImage(link, color, size);
        }
    }
}
