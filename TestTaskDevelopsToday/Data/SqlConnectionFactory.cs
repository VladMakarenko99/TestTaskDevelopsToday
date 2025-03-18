using Microsoft.Data.SqlClient;

namespace TestTaskDevelopsToday.Data;

public class SqlConnectionFactory
{
    private readonly string _connectionString =
        "Server=localhost,1433;Database=master;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;";

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}