using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace Library.MsgQueue
{
    public class RabbitMqClient<T> : IMsgQueueClient<T>
        where T : struct
    {
        private readonly ChannelSettings _outSettings;
        public const string HostName = "localhost";
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqClient(ChannelSettings inSettings, ChannelSettings outSettings)
        {
            _outSettings = outSettings;
            var factory = new ConnectionFactory { HostName = HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(outSettings.Exchange, outSettings.Type);
            _channel.ExchangeDeclare(inSettings.Exchange, inSettings.Type);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, inSettings.Exchange, string.Empty);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, args) => OnRecieveEventHandler?.Invoke(sender, args);
            _channel.BasicConsume(queueName, true, consumer);
        }

        public void Send(byte[] body, T option)
        {
            _channel.BasicPublish(_outSettings.Exchange, string.Empty, 
                new BasicProperties { CorrelationId = option.ToString() }, body);
        }

        public EventHandler<BasicDeliverEventArgs> OnRecieveEventHandler { get; set; }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }

    public enum ClientOption
    {
        File,
        Settings
    }

    public enum ServerOption
    {
        UpdateStatus,
        NewTimeout
    }
}
