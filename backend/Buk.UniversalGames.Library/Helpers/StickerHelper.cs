using System.Reflection;
using ImageMagick;
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

        public static byte[]? GetQRImage(string stickerCode, string color,  int size = 40)
        {
            // get qr link
            var link = GetStickerLink(stickerCode);

            // generate qr
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(qrCodeData);

            var colorValues = color.Split(",");

            var bytes = qrCode.GetGraphic(size, new byte[] { byte.Parse(colorValues[0]), byte.Parse(colorValues[1]), byte.Parse(colorValues[2]), 255}, new byte[]{0,0,0,0});

            var assembly = Assembly.GetExecutingAssembly();

            // prepare logo
            var logoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Buk.UniversalGames.Library.Resources.logo.png");

            using (var stream = new MemoryStream(bytes))
            {
                using (var image = new MagickImage(stream))
                {
                    if (logoStream != null)
                    {
                        using (var logo = new MagickImage(logoStream))
                        {
                            logo.Resize((int) (image.Width * 0.25), (int) (image.Height * 0.25));
                            image.Composite(logo, (int) (image.Width / 2 - logo.Width / 2) , (int)(image.Height / 2 - logo.Height / 2), CompositeOperator.Over);
                        }
                    }


                    return image.ToByteArray(MagickFormat.Png);
                }
            }
        }
    }
}
