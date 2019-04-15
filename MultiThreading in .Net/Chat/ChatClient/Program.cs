using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using SocketLibrary.Service;

namespace ChatClient
{
    class Program
    {
        public static List<string> Messages = new List<string>
        {
            "Is it OK to feed my dog the same thing that I feed my cat?",
            "Would you come here a moment?",
            "What was it that you gave him?",
            "I'll take care of your children tonight.",
            "Is it near your house?",
            "Tom fed his leftovers to his dog.",
            "I cooked supper last night.",
            "I have a migraine.",
            "She came very near to being run over by a car.",
        };

        static void Main(string[] args)
        {
            ISocketClient client = null;

            try
            {
                var isServerClosed = false;
                var random = new Random();
                client = new SocketClient();
                client.OnMessageRecieved += (obj, msg) => Console.WriteLine(msg);
                client.OnServerClosed += (obj, msg) => isServerClosed = true;

                Console.WriteLine("======================================");
                Console.WriteLine("Client was started...");
                Console.WriteLine("======================================");
                client.StartAsync();
                var count = random.Next(5, 10);
                var i = 0;
                while (i++ != count && !isServerClosed)
                {
                    client.SendAsync(Messages[random.Next(0, 9)]);
                    Thread.Sleep(random.Next(7000, 15000));
                }
            }
            catch (SocketException e) when(e.SocketErrorCode == SocketError.HostDown)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.StopAsync();
            }
        }
    }
}
