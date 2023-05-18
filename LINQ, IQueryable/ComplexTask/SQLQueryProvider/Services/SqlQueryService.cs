using System.Data.SqlClient;

namespace SQLQueryProvider.Services
{
    public class SqlQueryService : ISqlQueryService
    {
        private readonly string _connectionString;

        public SqlQueryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TResult> Execute<TResult>(string query)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var command = new SqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var result = new List<TResult>();
            while (reader.Read()) 
            {
                var type = typeof(TResult);
                var instance = Activator.CreateInstance(type);
                foreach (var property in type.GetProperties())
                {
                    property.SetValue(instance, reader.GetValue(reader.GetOrdinal(property.Name)));
                }
                result.Add((TResult)instance);
            }

            return result;
        }
    }
}
