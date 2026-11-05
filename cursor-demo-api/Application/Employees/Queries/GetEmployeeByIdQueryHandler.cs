using cursor_demo_api.Cqrs;
using cursor_demo_api.Data;
using cursor_demo_api.Models;
using Dapper;

namespace cursor_demo_api.Application.Employees.Queries;

public sealed class GetEmployeeByIdQueryHandler : IQueryHandler<GetEmployeeByIdQuery, Employee?>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    public GetEmployeeByIdQueryHandler(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetEmployeeByIdQueryHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<Employee?> HandleAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, Name, Age, Salary, PhoneNumber
            FROM dbo.Employees
            WHERE Id = @Id;
            """;

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            return await connection.QuerySingleOrDefaultAsync<Employee>(
                new CommandDefinition(sql, new { query.Id }, cancellationToken: cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to read employee {query.Id}.");
            throw;
        }
    }
}
