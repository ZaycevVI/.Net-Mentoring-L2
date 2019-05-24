using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessagingClient
{
    class Program
    {
        public static void Main()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", "fanout");

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queueName, "logs", string.Empty);

                channel.QueueDeclare("task_queue",
                    true,
                    false,
                    false,
                    null);

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] {0}", message);
                };
                channel.BasicConsume(queueName, true, consumer);

                channel.ExchangeDeclare("direct_logs",
                    "direct");
               
                channel.BasicPublish("direct_logs",
                    string.Empty,
                    null,
                    Encoding.UTF8.GetBytes("Hello World from client!"));

                Console.WriteLine(" [x] Sent '{0}'", "Hello World from client!");

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
