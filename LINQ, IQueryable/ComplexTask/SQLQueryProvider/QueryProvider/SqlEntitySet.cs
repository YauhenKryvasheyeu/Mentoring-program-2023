using SQLQueryProvider.Models;
using SQLQueryProvider.Services;
using System.Collections;
using System.Linq.Expressions;

namespace SQLQueryProvider.QueryProvider
{
    public class SqlEntitySet<T> : IQueryable<T> where T : BaseSqlEntity
    {
        protected readonly Expression Exp;
        protected readonly IQueryProvider QueryProvider;

        public SqlEntitySet(ISqlQueryService service)
        {
            Exp = Expression.Constant(this);
            QueryProvider = new SqlLinqProvider(service);
        }

        public Type ElementType => typeof(T);

        public Expression Expression => Exp;

        public IQueryProvider Provider => QueryProvider;

        public IEnumerator<T> GetEnumerator()
        {
            return QueryProvider.Execute<IEnumerable<T>>(Exp).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return QueryProvider.Execute<IEnumerable>(Exp).GetEnumerator();
        }
    }
}
