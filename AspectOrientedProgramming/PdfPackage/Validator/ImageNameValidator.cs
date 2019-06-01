using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PdfPackage.Validator
{
    public class ImageNameValidator : IValidator
    {
        private readonly Regex _nameTempalte = new Regex(@"\w+_\d+\.[a-z]+");

        public bool Validate(string fileName) => !string.IsNullOrWhiteSpace(fileName) &&
                   Path.HasExtension(fileName) &&
                   ImgExtensions.List.Contains(Path.GetExtension(fileName)) &&
                   _nameTempalte.IsMatch(fileName);
    }
}