using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Library.Pdf
{
    public class SharpImgToByteConverter : IImgToByteConverter
    {
        private readonly MemoryStream _memoryStream = new MemoryStream();

        public async Task<byte[]> ConvertAsync(IEnumerable<string> imgPaths)
        {
            var document = new PdfDocument();

            foreach (var imgPath in imgPaths)
            {
                // Create an empty page or load existing
                var page = document.AddPage();

                // Get an XGraphics object for drawing
                var gfx = XGraphics.FromPdfPage(page);
                DrawImage(gfx, imgPath, 0, 0, page.Width.Value, page.Height.Value);
            }

            await Task.Run(() => document.Save(_memoryStream));

            return _memoryStream.ToArray();
        }

        private void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, double width, double height)
        {
            var image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }
    }
}