using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace PdfPackage.Pdf
{
    public class PdfSharpGenerator : IPdfGenerator
    {
        private const int Attempts = 4;
        private const int Delay = 1; // minutes
        private const string BrokenDir = "C:\\broken";

        public Task GenerateAsync(string pdfPath, params string[] imgPaths)
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

            return Task.Run(() =>
            {
                try
                {
                    TrySave(() => document.Save(pdfPath), Attempts);
                }
                catch (Exception)
                {
                    MoveTo(BrokenDir, imgPaths);
                }
            });
        }

        private void MoveTo(string dst, params string[] paths)
        {
            foreach (var path in paths)
            {
                TrySave(() => System.IO.File.Move(path, $"{dst}\\{Path.GetFileName(path)}"), Attempts);
            }
        }

        private void TrySave(Action func, int attempts)
        {
            var attempt = 0;

            while (true)
            {
                if (attempt >= attempts)
                    break;

                try
                {
                    func();
                    break;
                }
                catch (IOException)
                {
                    Thread.Sleep(Delay * 60000);
                }

                attempt++;
            }
        }

        private void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, double width, double height)
        {
            var image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }
    }
}