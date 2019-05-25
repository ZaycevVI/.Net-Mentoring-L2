using System;
using System.Text;
using Library.FolderListener;
using Library.MsgQueue;
using Library.Pdf;

namespace MessagingClient
{
    class Program
    {
        private const string InDir = "C:\\InDir";
        public static void Main()
        {
            var msgClient = new RabbitMqClient<ClientOption>(
                new ChannelSettings("server", ChannelType.Fanout),
                new ChannelSettings("client", ChannelType.Direct));
            var pdfGenerator = new SharpImgToByteConverter();

            var dirListener = new DirectoryListener(new DirectoryListenerSettings
            {
                InDir = InDir,
                Timeout = 100000
            });

            var listener = new DirectoryListenerProxy(dirListener, msgClient, pdfGenerator);
            listener.Start();
            Console.ReadLine();
        }
    }
}
