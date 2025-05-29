using Microsoft.Data.SqlClient;
using TaskManagerAPI.Data.Interface;

namespace TaskManagerAPI.Data.Implementation;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString) => _connectionString = connectionString;

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}