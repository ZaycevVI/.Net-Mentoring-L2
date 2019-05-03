using System;
using System.Collections.Generic;
using System.Linq;
using HttpServer.Data;
using Library;

namespace HttpServer.Converter
{
    public class QueryConverter
    {
        private readonly DataContext _context;

        public QueryConverter(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Entity> Convert(List<QueryDto> queries)
        {
            var entities = _context.Entities;
            var predicates = queries.Select(HandleQuery);

            return entities.Where(e => predicates.Select(p => p.Invoke(e)).All(flag => flag));
        }

        private Func<Entity, bool> HandleQuery(QueryDto query)
        {
            switch (query.MethodType)
            {
                case MethodType.Contains:
                    return entity => GetValue(entity, query.FieldName).Contains(query.Value); 
                case MethodType.EndsWith:
                    return entity => GetValue(entity, query.FieldName).EndsWith(query.Value);
                case MethodType.StartsWith:
                    return entity => GetValue(entity, query.FieldName).StartsWith(query.Value);
                case MethodType.Equals:
                    return entity => GetValue(entity, query.FieldName).Equals(query.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string GetValue(Entity entity, string propertyName) =>
            (string)entity.GetType().GetProperty(propertyName)?.GetValue(entity, null);
    }
}
