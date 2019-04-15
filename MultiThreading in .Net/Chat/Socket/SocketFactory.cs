using System.Net;
using System.Net.Sockets;

namespace SocketLibrary
{
    public class SocketFactory
    {
        public const string HostName = "localhost";
        public const int Port = 11000;

        public static Socket Create(out IPEndPoint endPoint)
        {
            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            var host = Dns.GetHostEntry(HostName);
            var ipAddress = host.AddressList[0];
            endPoint = new IPEndPoint(ipAddress, Port);

            // Create a Socket that will use Tcp protocol      
            return new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
