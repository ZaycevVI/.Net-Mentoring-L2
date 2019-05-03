using System.Collections.Generic;
using System.Text;
using HttpServer.Converter;
using HttpServer.Data;
using HttpServer.Factory;
using Library;
using Newtonsoft.Json;

namespace HttpServer.Server
{
    public class HttpListener : IHttpListener
    {
        private readonly System.Net.HttpListener _listener;
        private readonly QueryConverter _converter = new QueryConverter(new DataContext());

        public HttpListener(params string[] addresses)
        {
            _listener = HttpListenerFactory.Create(addresses);
        }

        public void Start()
        {
            _listener.Start();

            while (true)
            {
                var context = _listener.GetContext();
                var buffer = new byte[context.Request.ContentLength64];
                context.Request.InputStream.Read(buffer, 0, buffer.Length);
                var queries = JsonConvert.DeserializeObject<List<QueryDto>>(Encoding.UTF8.GetString(buffer));
                var results = _converter.Convert(queries);
                var response = context.Response;
                var resultString = JsonConvert.SerializeObject(results);
                buffer = Encoding.UTF8.GetBytes(resultString);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);

                context.Response.Close();
            }
        }
    }
}