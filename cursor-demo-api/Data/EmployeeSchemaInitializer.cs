using Dapper;

namespace cursor_demo_api.Data;

public sealed class EmployeeSchemaInitializer
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<EmployeeSchemaInitializer> _logger;

    public EmployeeSchemaInitializer(
        ISqlConnectionFactory connectionFactory,
        ILogger<EmployeeSchemaInitializer> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task EnsureCreatedAsync(CancellationToken cancellationToken)
    {
        const string sql = """
            IF OBJECT_ID(N'dbo.Employees', N'U') IS NULL
            BEGIN
                CREATE TABLE dbo.Employees
                (
                    Id INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
                    Name NVARCHAR(200) NOT NULL,
                    Age INT NOT NULL,
                    Salary DECIMAL(18, 2) NOT NULL,
                    PhoneNumber NVARCHAR(30) NOT NULL
                );
            END
            """;

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync(new CommandDefinition(sql, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to initialize the Employees table.");
            throw;
        }
    }
}
