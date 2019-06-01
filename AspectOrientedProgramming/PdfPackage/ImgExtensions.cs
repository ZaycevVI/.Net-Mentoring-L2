using System.Collections.Generic;

namespace PdfPackage
{
    public static class ImgExtensions
    {
        public static IReadOnlyCollection<string> List = new List<string>
        {
            ".png", ".jpg", "jpeg", ".bmp"
        };
    }
}