using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TestTaskDevelopsToday.Data;

public class SqlConnectionFactory
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddUserSecrets<SqlConnectionFactory>() 
        .Build();

    public SqlConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string is missing from user secrets.");
        }

        return new SqlConnection(connectionString);
    }
}