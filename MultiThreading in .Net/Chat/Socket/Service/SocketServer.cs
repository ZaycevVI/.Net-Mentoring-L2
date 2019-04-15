using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace SocketLibrary.Service
{
    public class SocketServer : ISocketServer
    {
        public const int MaxClients = 3;

        private readonly Socket _socket;
        private readonly MessageStorage _storage;
        private readonly ConcurrentDictionary<string, Socket> _connectedSockets;
        private readonly byte[] _buffer = new byte[1024];

        public SocketServer()
        {
            _socket = SocketFactory.Create(out var endPoint);
            _storage = new MessageStorage();
            _connectedSockets = new ConcurrentDictionary<string, Socket>();
            _socket.Bind(endPoint);
            _socket.Listen(MaxClients);
        }

        public async Task StartAsync()
        {
            do
            {
                var newConnection = await _socket.AcceptAsync();
                HandleConnectedSocketAsync(newConnection);
            } while (true);
        }

        protected async Task HandleConnectedSocketAsync(Socket newClient)
        {
            var clientId = newClient.RemoteEndPoint.ToString();
            _connectedSockets[clientId] = newClient;

            await HandShakeAsync(clientId);
            await ListenAsync(newClient);

            _connectedSockets.TryRemove(clientId, out var socket);
            var msg = $"[Server]: {clientId} close connection.";
            await NotifyOnMsgReceived(msg);
        }

        public async Task StopAsync()
        {
            await NotifyOnMsgReceived("[Server]: Server was closed.");
            _connectedSockets.Clear();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public event EventHandler<string> OnMessageRecieved;

        private async Task NotifyOnMsgReceived(string msg, params string[] exceptClientIds)
        {
            await SendToAll(msg, exceptClientIds);
            OnMessageRecieved?.Invoke(this, msg);
        }

        private async Task HandShakeAsync(string clientId)
        {
            var msg = $"[Server]: {clientId} was successfully connected to the server.";
            var msgTasks = _storage.Messages.Any() ?
                _storage.Messages.Select(m => SendAsync($"[Server(top 10 cached)]: {m}{Environment.NewLine}", clientId)).ToArray()
                : new[] { SendAsync("[Server(top 10 cached)]: Empty cache.", clientId) };
            await Task.WhenAll(Task.WhenAll(msgTasks), SendToAll(msg, clientId));
            _storage.Push(msg);
        }

        private async Task ListenAsync(Socket socket)
        {
            var senderId = socket.RemoteEndPoint.ToString();
            int bytesRec;
            do
            {
                bytesRec = await socket.ReceiveAsync(new ArraySegment<byte>(_buffer), SocketFlags.None);
                var msg = DataConverter.GetString(_buffer, bytesRec);
                await NotifyOnMsgReceived($"[{senderId}]: {msg}", senderId);
                _storage.Push(msg);
            } while (bytesRec != 0);
        }

        public async Task SendAsync(string msg, string clientId)
        {
            if (_connectedSockets.TryGetValue(clientId, out var socket))
            {
                await socket.SendAsync(DataConverter.GetBytes(msg), SocketFlags.None);
            }
        }

        public async Task SendToAll(string msg, params string[] exceptSenderIds)
        {
            exceptSenderIds = exceptSenderIds ?? Array.Empty<string>();
            var tasks = new List<Task>();

            foreach (var socket in _connectedSockets)
            {
                if (exceptSenderIds.All(s => s != socket.Value.RemoteEndPoint.ToString()))
                    tasks.Add(SendAsync(msg, socket.Key));
            }

            await Task.WhenAll(tasks.ToArray());
        }
    }
}
