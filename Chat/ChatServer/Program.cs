using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SocketLibrary;
using SocketLibrary.Service;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            StartServer();
        }

        public static void StartServer()
        {
            ISocket server = null;

            try
            {
                server = new SocketServer();
                server.OnMessageRecieved += (obj, msg) => Console.WriteLine(msg);
                Console.WriteLine("======================================");
                Console.WriteLine("Server was started...");
                Console.WriteLine("======================================");
                server.StartAsync();
                Console.WriteLine("\n Press any key to stop server...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                server.StopAsync();
            }
        }
    }
}
