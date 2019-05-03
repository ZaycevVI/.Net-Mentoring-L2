using System;
using HttpServer.Server;

namespace HttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IHttpListener listener = new HttpListener("http://127.0.0.1/");
            Console.WriteLine("Listening...");
            listener.Start();
        }
    }
}
