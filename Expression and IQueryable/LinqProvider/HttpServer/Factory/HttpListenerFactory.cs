using System.Net;

namespace HttpServer.Factory
{
    public class HttpListenerFactory
    {
        public static HttpListener Create(params string[] addresses)
        {
            var httpListener = new HttpListener();

            foreach (var address in addresses)
            {
                httpListener.Prefixes.Add(address);
            }

            return httpListener;
        }
    }
}