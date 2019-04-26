using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MapperSample
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var destination = Expression.New(typeof(TDestination));

            var sourceProperties = GetProperties(sourceParam.Type);
            var destinationProperties = GetProperties(destination.Type);

            var memberBindings = (from sourceProperty 
                               in sourceProperties
                               where destinationProperties.Any(d => d.Name == sourceProperty.Name)
                               let left = destinationProperties.First(i => i.Name == sourceProperty.Name)
                               let right = Expression.PropertyOrField(sourceParam, sourceProperty.Name)
                               select Expression.Bind(left, right)).ToList();

            var memberInit = Expression.MemberInit(destination, memberBindings);

            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(
                memberInit, sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type) => type.GetProperties(
                BindingFlags.Public | BindingFlags.GetProperty
                | BindingFlags.Instance | BindingFlags.SetProperty);
    }
}