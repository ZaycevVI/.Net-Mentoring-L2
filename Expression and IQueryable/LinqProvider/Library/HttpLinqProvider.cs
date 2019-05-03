using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using Newtonsoft.Json;

namespace Package
{
    public class HttpLinqProvider : IQueryProvider
    {
        private readonly HttpClient _httpClient;
        private const string Host = "http://127.0.0.1/";

        public HttpLinqProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public IQueryable CreateQuery(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new HttpQuery<TElement>(expression, this);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var visitor = new HttpExpressionVisitor();
            var entity = visitor.Translate(expression);
            var content = JsonConvert.SerializeObject(entity, Formatting.Indented);
            var result = _httpClient.PostAsync(Host, new StringContent(content)).Result;
            var stringResponse = result.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<TResult>(stringResponse);
        }
    }
}
