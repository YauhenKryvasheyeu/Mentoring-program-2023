using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>(Action<TSource, TDestination> custom = null)
        {
            var sourceParam = Expression.Parameter(typeof(TSource));

            var destination = Expression.New(typeof(TDestination));

            var sourceProperties = sourceParam.Type.GetProperties();

            var propertyAssignments = new List<MemberAssignment>();

            foreach (var destProperty in destination.Type.GetProperties())
            {
                var sourceProperty = sourceProperties
                    .SingleOrDefault(prop => prop.Name == destProperty.Name && prop.PropertyType == destProperty.PropertyType);
                if (sourceProperty != null)
                {
                    var propertyExpression = Expression.Property(sourceParam, sourceProperty);
                    propertyAssignments.Add(Expression.Bind(destProperty, propertyExpression));
                }
            }
            var memberInit = Expression.MemberInit(destination, propertyAssignments);

            var mapFunction =
                Expression.Lambda<Func<TSource, TDestination>>(
                    memberInit,
                    sourceParam
                );

            return new Mapper<TSource, TDestination>(mapFunction.Compile(), custom);
        }
    }
}
