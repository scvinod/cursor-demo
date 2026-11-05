using cursor_demo_api.Cqrs;
using cursor_demo_api.Data;
using cursor_demo_api.Models;
using Dapper;

namespace cursor_demo_api.Application.Employees.Queries;

public sealed class GetEmployeesQueryHandler : IQueryHandler<GetEmployeesQuery, IReadOnlyList<Employee>>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<GetEmployeesQueryHandler> _logger;

    public GetEmployeesQueryHandler(
        ISqlConnectionFactory connectionFactory,
        ILogger<GetEmployeesQueryHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Employee>> HandleAsync(GetEmployeesQuery query, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT Id, Name, Age, Salary, PhoneNumber
            FROM dbo.Employees
            ORDER BY Id;
            """;

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            var employees = await connection.QueryAsync<Employee>(
                new CommandDefinition(sql, cancellationToken: cancellationToken));

            return employees.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to read employees.");
            throw;
        }
    }
}
