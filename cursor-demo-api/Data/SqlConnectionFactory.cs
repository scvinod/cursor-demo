using Microsoft.Data.SqlClient;

namespace cursor_demo_api.Data;

public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    private const string ConnectionStringName = "EmployeeDatabase";
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' was not found.");
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
