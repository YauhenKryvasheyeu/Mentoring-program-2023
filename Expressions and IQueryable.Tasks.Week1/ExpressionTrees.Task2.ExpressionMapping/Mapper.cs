using System;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class Mapper<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> _mapFunction;
        private readonly Action<TSource, TDestination> _custom;

        internal Mapper(Func<TSource, TDestination> func, Action<TSource, TDestination> custom = null)
        {
            _custom = custom;
            _mapFunction = func;
        }

        public TDestination Map(TSource source)
        {
            var destination = _mapFunction(source);
            _custom?.Invoke(source, destination);
            return destination;
        }
    }
}
