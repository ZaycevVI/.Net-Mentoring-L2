using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;

#pragma warning disable CS4014

namespace SocketLibrary.Service
{
    public class SocketClient : ISocketClient
    {
        private readonly IPEndPoint _endPoint;
        private readonly Socket _socket;
        private readonly byte[] _buffer = new byte[1024];
        private const int Timeout = 3000;
        private bool _isClosed = true;

        public SocketClient()
        {
            _socket = SocketFactory.Create(out _endPoint);
            _socket.ReceiveTimeout = Timeout;
        }

        public async Task StartAsync()
        {
            _isClosed = false;
            await _socket.ConnectAsync(_endPoint);
            await ListenAsync();
        }

        private async Task ListenAsync()
        {
            int bytesRec;
            do
            {
                bytesRec = await _socket.ReceiveAsync(new ArraySegment<byte>(_buffer), SocketFlags.None);
                var msg = $"[Client received]: {DataConverter.GetString(_buffer, bytesRec)}";
                NotifyOnMsgReceived(msg);

                if (IsServerClosed(msg))
                {
                    // Server is closed!!!
                    OnServerClosed?.Invoke(this, "Server was terminated.");
                }
            } while (bytesRec != 0);
        }

        public async Task SendAsync(string msg)
        {
            await _socket.SendAsync(DataConverter.GetBytes(msg), SocketFlags.None);
            NotifyOnMsgReceived($"[Clien send]: {msg}");
        }

        public Task StopAsync()
        {
            if (!_isClosed)
            {
                NotifyOnMsgReceived("[Client]: Client is closed.");
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Close();
                _isClosed = true;
            }
            
            return Task.CompletedTask;
        }

        public event EventHandler<string> OnMessageRecieved;
        public event EventHandler<string> OnServerClosed;

        private void NotifyOnMsgReceived(string msg)
        {
            OnMessageRecieved?.Invoke(this, msg);
        }

        private bool IsServerClosed(string msg) => msg.Contains("Server was closed");
    }
}
