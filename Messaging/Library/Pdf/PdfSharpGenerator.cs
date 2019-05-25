using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PdfSharp.Pdf;

namespace Library.Pdf
{
    public class PdfSharpGenerator : IPdfGenerator
    {
        private readonly string _outDir;

        public PdfSharpGenerator(string outDir = "C:\\OutDir")
        {
            _outDir = outDir;

            if (!Directory.Exists(_outDir))
                Directory.CreateDirectory(_outDir);
        }

        public async Task GenerateAsync(byte[] file)
        {
            await Task.Run(() => File.WriteAllBytes($"{_outDir}\\{Guid.NewGuid()}.pdf", file));
        }
    }
}