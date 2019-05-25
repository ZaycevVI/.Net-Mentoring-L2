using System;
using System.Text;
using System.Threading;
using Library.MsgQueue;
using Library.Pdf;
using RabbitMQ.Client.Events;

namespace MessagingServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            var msgClient = new RabbitMqClient<ServerOption>(
                new ChannelSettings("client", ChannelType.Direct),
                new ChannelSettings("server", ChannelType.Fanout));
            var pdfGenerator = new PdfSharpGenerator();
            using (var service = new ServerService(msgClient, pdfGenerator))
            {
                service.Start();
                Console.ReadLine();
            }
        }

        public class ServerService : IDisposable
        {
            public RabbitMqClient<ServerOption> MsgQueueClient { get; }
            public PdfSharpGenerator PdfGenerator { get; }
            private Timer _timer;
            private readonly Random _random = new Random();

            public ServerService(
                RabbitMqClient<ServerOption> msgQueueClient, 
                PdfSharpGenerator pdfGenerator)
            {
                MsgQueueClient = msgQueueClient;
                PdfGenerator = pdfGenerator;
            }

            public void Start()
            {
                _timer = new Timer(TimerCallback, null, 0, 10000);
                MsgQueueClient.OnRecieveEventHandler += OnRecieveEventHandler;
            }

            private void OnRecieveEventHandler(object o, BasicDeliverEventArgs eventArgs)
            {
                if (eventArgs.BasicProperties.CorrelationId == ClientOption.Settings.ToString())
                {
                    Console.WriteLine($"[Server]: Settings status was recieved:{Environment.NewLine}" +
                                      $"{Encoding.UTF8.GetString(eventArgs.Body)}");
                }
                else if (eventArgs.BasicProperties.CorrelationId == ClientOption.File.ToString())
                {
                    Console.WriteLine("[Server]: Start writing new file.");
                    PdfGenerator.GenerateAsync(eventArgs.Body);
                }
            }

            private void TimerCallback(object state)
            {
                var refreshStatus = _random.Next(0, 5);
                var setNewTimeout = _random.Next(0, 5);

                if (refreshStatus == 1)
                {
                    Console.WriteLine("[Server]: Send msg to report about status.");
                    MsgQueueClient.Send(null, ServerOption.UpdateStatus);
                }

                if (setNewTimeout == 1)
                {
                    var timeout = _random.Next(10000, 100000);
                    Console.WriteLine($"[Server]: Send msg with new timeout: {timeout}");
                    MsgQueueClient.Send(BitConverter.GetBytes(timeout), ServerOption.NewTimeout);
                }
            }

            public void Dispose()
            {
                _timer?.Dispose();
                MsgQueueClient?.Dispose();
            }
        }
    }
}
