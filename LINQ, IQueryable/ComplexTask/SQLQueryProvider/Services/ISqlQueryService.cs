namespace SQLQueryProvider.Services
{
    public interface ISqlQueryService
    {
        IEnumerable<TResult> Execute<TResult>(string query);
    }
}
