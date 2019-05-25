using System;
using System.Collections.Generic;

namespace Library.FolderListener
{
    public class PdfFileEventArg : EventArgs
    {
        public List<string> Imgs { get; }

        public PdfFileEventArg(List<string> imgs)
        {
            Imgs = imgs;
        }
    }

    public interface IDirectoryListener
    {
        void Start();
        void Stop();
        DirectoryListenerSettings Settings { get; }
        EventHandler<PdfFileEventArg> OnFileReady { get; set; }
    }
}