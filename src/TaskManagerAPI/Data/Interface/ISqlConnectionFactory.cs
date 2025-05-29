using Microsoft.Data.SqlClient;

namespace TaskManagerAPI.Data.Interface;

public interface ISqlConnectionFactory
{
    SqlConnection GetConnection();
}