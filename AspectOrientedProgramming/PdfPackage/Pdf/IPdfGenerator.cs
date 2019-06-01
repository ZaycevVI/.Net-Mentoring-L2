using System.Threading.Tasks;

namespace PdfPackage.Pdf
{
    public interface IPdfGenerator
    {
        Task GenerateAsync(string pdfPath, params string[] imgPaths);
    }
}