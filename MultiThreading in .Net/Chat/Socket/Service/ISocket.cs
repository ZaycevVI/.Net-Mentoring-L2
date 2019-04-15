using System;
using System.Threading.Tasks;

namespace SocketLibrary.Service
{
    public interface ISocket
    {
        Task StartAsync();

        Task StopAsync();

        event EventHandler<string> OnMessageRecieved;
    }

    public interface ISocketClient : ISocket
    {
        Task SendAsync(string msg);

        event EventHandler<string> OnServerClosed;
    }

    public interface ISocketServer : ISocket
    {
        Task SendAsync(string msg, string clientId);
    }
}