using System;
using RabbitMQ.Client.Events;

namespace Library.MsgQueue
{
    public interface IMsgQueueClient<T> : IDisposable
        where T : struct
    {
        void Send(byte[] body, T option);
        EventHandler<BasicDeliverEventArgs> OnRecieveEventHandler { get; set; }
    }
}