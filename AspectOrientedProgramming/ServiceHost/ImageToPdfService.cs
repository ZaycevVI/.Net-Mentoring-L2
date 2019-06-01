using PdfPackage.File;
using Topshelf;

namespace WindowsService
{
    public class ImageToPdfService : ServiceControl
    {
        private readonly IDirectoryListener _listener;

        public ImageToPdfService(IDirectoryListener listener)
        {
            _listener = listener;
        }

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