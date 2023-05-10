using System.Reflection;
using ImageMagick;
using QRCoder;

namespace Buk.UniversalGames.Library.Helpers
{
    public class QRHelper
    {
        public static byte[]? GetQRImage(string link, string color,  int size = 40, int circleSize = 185)
        {
            // generate qr
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);

            var qrCode = new PngByteQRCode(qrCodeData);

            var bytes = qrCode.GetGraphic(size, new byte[] { 90, 32, 39, 255 }, new byte[] { 0, 0, 0, 0 });

            // prepare logo
            var logoStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@"Buk.UniversalGames.Library.Resources.logo.png");

            using var stream = new MemoryStream(bytes);
            using (var image = new MagickImage(stream))
            {
                image.Draw(new DrawableFillColor(new MagickColor(color)),

                    new DrawableCircle(image.Width / 2,
                        image.Height / 2,
                        (image.Width / 2) + circleSize,
                        (image.Height / 2) + 1)
                );

                if (logoStream != null)
                {
                    using (var logo = new MagickImage(logoStream))
                    {
                        logo.Resize((int)(image.Width * 0.20), (int)(image.Height * 0.20));
                        image.Composite(logo, (image.Width / 2 - logo.Width / 2), (image.Height / 2 - logo.Height / 2), CompositeOperator.Over);
                    }
                }

                return image.ToByteArray(MagickFormat.Png);
            }
        }
    }
}
