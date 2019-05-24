using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Topshelf;

namespace MessagingServer
{
    class Program
    {
        public static void Main(string[] args)
        {
            //HostFactory.Run(conf =>
            //{
            //    conf.SetServiceName("ImgToPdfService");
            //    conf.SetDisplayName("Image To Pdf Service");
            //    conf.Service<ImageToPdfService>();
            //});

            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", "fanout");
                channel.ExchangeDeclare("direct_logs", "direct");

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, "direct_logs", string.Empty);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body1 = ea.Body;
                    var message1 = Encoding.UTF8.GetString(body1);
                    var routingKey = ea.RoutingKey;
                    Console.WriteLine(" [x] Received '{0}':'{1}'",
                        routingKey, message1);
                };
                channel.BasicConsume(queueName,
                    true,
                    consumer);

                var message = GetMessage(new[] { "Hello", " World" });
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs",
                    string.Empty,
                    null,
                    body);
                Console.WriteLine(" [x] Sent {0}", message);
                Console.ReadLine();
            }
        }

        private static string GetMessage(string[] args)
        {
            return (args.Length > 0
                ? string.Join(" ", args)
                : "info: Hello World!");
        }
    }
}
