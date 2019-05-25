using System.Threading.Tasks;

namespace Library.Pdf
{
    public interface IPdfGenerator
    {
        Task GenerateAsync(byte[] file);
    }
}