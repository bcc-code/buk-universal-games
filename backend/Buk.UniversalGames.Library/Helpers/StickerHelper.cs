using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;

namespace Buk.UniversalGames.Library.Helpers
{
    public class StickerHelper
    {
        public static string GetStickerLink(string stickerCode)
        {
            return $"https://universalgames.buk.no/#/scan/{stickerCode}";
        }

        public static string GetStickerQRLInk(string stickerCode)
        {
            return $"https://universalgames.buk.no/api/QR/{stickerCode}";
        }

        public static byte[]? GetQRImage(string stickerCode, int size = 20)
        {
            var link = GetStickerLink(stickerCode);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(qrCodeData);
            var bytes = qrCode.GetGraphic(size, new byte[]{153,78,62,255}, new byte[]{0,0,0,0});

            Bitmap bitmap;
            using (var stream = new MemoryStream(bytes))
            {
                bitmap = new Bitmap(stream);
            }
            return bytes;

          /*  var graphics = Graphics.FromImage(bitmap);

            using var bitmapToBytesStream = new MemoryStream();
            bitmap.Save(bitmapToBytesStream, ImageFormat.Png);
            return bitmapToBytesStream.ToArray();*/




            /*         var qrCode = new QRCode(qrCodeData);
            var bitmap = qrCode.GetGraphic(size); //, Color.FromArgb(255,153,78,62), Color.FromArgb(0,0,0,0), Images.logo, 25);
   */
          /*  var graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            var rectangle = new Rectangle(75, bitmap.Height-85, bitmap.Width-150, 70);
            graphics.DrawString(GetStickerLink(stickerCode), new Font("Tahoma", 16, FontStyle.Bold), Brushes.Black, rectangle, stringFormat);
            graphics.Flush();*/
            
         /*   byte[] result;
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                result = stream.ToArray();
            }
            return result;*/
;        }
    }
}
