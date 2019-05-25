using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Pdf
{
    public interface IImgToByteConverter
    {
        Task<byte[]> ConvertAsync(IEnumerable<string> imgPaths);
    }
}