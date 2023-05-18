using SQLQueryProvider.Helpers;
using SQLQueryProvider.Services;
using System.Linq.Expressions;

namespace SQLQueryProvider.QueryProvider
{
    public class SqlLinqProvider : IQueryProvider
    {
        private readonly ISqlQueryService _service;

        public SqlLinqProvider(ISqlQueryService service)
        {
            _service = service;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new SqlQuery<TElement>(expression, this);
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            Type itemType = TypeHelper.GetElementType(expression.Type);
            var translator = new ExpressionToSqlQueryTranslator();
            string queryString = translator.Translate(expression);

            return (TResult)_service
                .GetType()
                .GetMethod(nameof(_service.Execute))
                .MakeGenericMethod(TypeHelper.GetElementType(expression.Type))
                .Invoke(_service, new[] { queryString });
        }
    }
}
