using QRCoder;

namespace Buk.UniversalGames.Library.Helpers
{
    public class StickerHelper
    {
        public static string GetStickerLink(string stickerCode)
        {
            return $"https://universalgames.buk.no/#/scan/{stickerCode}";
        }

        public static string GetStickerQRLInk(string stickerCode, int size = 20)
        {
            return $"https://universalgames.buk.no/api/QR/{stickerCode}/{size}";
        }

        public static byte[]? GetQRImage(string stickerCode, int size = 20)
        {
            var link = GetStickerLink(stickerCode);
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            return qrCode.GetGraphic(size);
        }
    }
}
