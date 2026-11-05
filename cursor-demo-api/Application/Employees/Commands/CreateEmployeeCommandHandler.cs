using cursor_demo_api.Cqrs;
using cursor_demo_api.Data;
using cursor_demo_api.Models;
using Dapper;

namespace cursor_demo_api.Application.Employees.Commands;

public sealed class CreateEmployeeCommandHandler : ICommandHandler<CreateEmployeeCommand, Employee>
{
    private readonly ISqlConnectionFactory _connectionFactory;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        ISqlConnectionFactory connectionFactory,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<Employee> HandleAsync(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO dbo.Employees (Name, Age, Salary, PhoneNumber)
            OUTPUT INSERTED.Id, INSERTED.Name, INSERTED.Age, INSERTED.Salary, INSERTED.PhoneNumber
            VALUES (@Name, @Age, @Salary, @PhoneNumber);
            """;

        var parameters = new
        {
            Name = command.Name.Trim(),
            command.Age,
            command.Salary,
            PhoneNumber = command.PhoneNumber.Trim()
        };

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            var employee = await connection.QuerySingleAsync<Employee>(
                new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));

            _logger.LogInformation($"Created employee {employee.Id}.");
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to add employee {command.Name}.");
            throw;
        }
    }
}
