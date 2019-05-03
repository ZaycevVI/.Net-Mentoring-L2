using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;

namespace Package
{
    public class HttpQuery<T> : IQueryable<T>
    {
        public HttpQuery()
        {
            Expression = Expression.Constant(this);
            Provider = new HttpLinqProvider(new HttpClient());
        }

        internal HttpQuery(Expression expression, HttpLinqProvider provider)
        {
            Expression = expression;
            Provider = provider;
        }

        public Type ElementType => typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Provider.Execute<IEnumerable>(Expression).GetEnumerator();
        }
    }
}
