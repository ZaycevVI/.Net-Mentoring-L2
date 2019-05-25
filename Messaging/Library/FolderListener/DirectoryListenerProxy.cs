using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library.MsgQueue;
using Library.Pdf;
using RabbitMQ.Client.Events;

namespace Library.FolderListener
{
    public class DirectoryListenerProxy : IDirectoryListener
    {
        private Guid Guid { get; }
        public IDirectoryListener DirectoryListener { get; }
        public IImgToByteConverter PdfGenerator { get; }
        public IMsgQueueClient<ClientOption> MsgQueueClient { get; }
        public DirectoryListenerSettings Settings => DirectoryListener.Settings;

        private readonly Timer _timer;

        public DirectoryListenerProxy(
            IDirectoryListener directoryListener,
            IMsgQueueClient<ClientOption> msgQueueClient,
            IImgToByteConverter pdfGenerator)
        {
            DirectoryListener = directoryListener;
            MsgQueueClient = msgQueueClient;
            PdfGenerator = pdfGenerator;
            directoryListener.OnFileReady += OnFileReadyHandler;
            MsgQueueClient.OnRecieveEventHandler += OnRecieveEventHandler;
            _timer = new Timer(TimerCallback, null, 0, 10000);
            Guid = Guid.NewGuid();
        }

        private void OnRecieveEventHandler(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            if (basicDeliverEventArgs.BasicProperties.CorrelationId == ServerOption.UpdateStatus.ToString())
            {
                SendMsg($"[Update status]:{Environment.NewLine}{Msg}");
            }
            else if (basicDeliverEventArgs.BasicProperties.CorrelationId == ServerOption.NewTimeout.ToString())
            {
                DirectoryListener.Settings.Timeout = BitConverter.ToInt32(basicDeliverEventArgs.Body, 0);
            }
            
        }

        private void TimerCallback(object state)
        {
            SendMsg(Msg);
        }

        private void OnFileReadyHandler(object sender, PdfFileEventArg pdfFileEventArg)
        {
            Task.Run(() =>
            {
                var body = PdfGenerator.ConvertAsync(
                    pdfFileEventArg.Imgs
                    .Select(img => $"{DirectoryListener.Settings.InDir}\\{img}"))
                    .Result;
                MsgQueueClient.Send(body, ClientOption.File);
            });

            OnFileReady?.Invoke(this, pdfFileEventArg);
        }

        public void Start()
        {
            DirectoryListener.Start();
        }

        public void Stop()
        {
            DirectoryListener.Stop();
        }

        public EventHandler<PdfFileEventArg> OnFileReady { get; set; }

        private void SendMsg(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg);
            MsgQueueClient.Send(body, ClientOption.Settings);
        }

        private string Msg => $"=========================={Environment.NewLine}" +
                              $"[{Guid}]:{Environment.NewLine}" +
                              $"Timeout: {DirectoryListener.Settings.Timeout}{Environment.NewLine}" +
                              $"Status: {DirectoryListener.Settings.CurrentStatus}{Environment.NewLine}" +
                              $"=========================={Environment.NewLine}";
    }
}