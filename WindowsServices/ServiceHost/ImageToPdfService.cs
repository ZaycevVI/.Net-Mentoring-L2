using PdfPackage.File;
using Topshelf;

namespace WindowsService
{
    public class ImageToPdfService : ServiceControl
    {
        private const string InDir = "C:\\InDir";
        private const string OutDir = "C:\\OutDir";
        private readonly IDirectoryListener _listener = new DirectoryListener(InDir, OutDir);

        public bool Start(HostControl hostControl)
        {
            _listener.Start();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _listener.Stop();
            return true;
        }
    }
}