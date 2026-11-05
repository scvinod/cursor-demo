using cursor_demo_api.Application.Employees.Commands;
using cursor_demo_api.Application.Employees.Queries;
using cursor_demo_api.Cqrs;
using cursor_demo_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace cursor_demo_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ICommandHandler<CreateEmployeeCommand, Employee> _createEmployeeCommandHandler;
    private readonly IQueryHandler<GetEmployeesQuery, IReadOnlyList<Employee>> _getEmployeesQueryHandler;
    private readonly IQueryHandler<GetEmployeeByIdQuery, Employee?> _getEmployeeByIdQueryHandler;

    public EmployeesController(
        ICommandHandler<CreateEmployeeCommand, Employee> createEmployeeCommandHandler,
        IQueryHandler<GetEmployeesQuery, IReadOnlyList<Employee>> getEmployeesQueryHandler,
        IQueryHandler<GetEmployeeByIdQuery, Employee?> getEmployeeByIdQueryHandler)
    {
        _createEmployeeCommandHandler = createEmployeeCommandHandler;
        _getEmployeesQueryHandler = getEmployeesQueryHandler;
        _getEmployeeByIdQueryHandler = getEmployeeByIdQueryHandler;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Employee>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployees(CancellationToken cancellationToken)
    {
        var employees = await _getEmployeesQueryHandler.HandleAsync(new GetEmployeesQuery(), cancellationToken);
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Employee>> GetEmployeeById(int id, CancellationToken cancellationToken)
    {
        var employee = await _getEmployeeByIdQueryHandler.HandleAsync(new GetEmployeeByIdQuery(id), cancellationToken);
        if (employee is null)
        {
            return NotFound(new { code = "employee_not_found", message = $"Employee {id} was not found." });
        }

        return Ok(employee);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Employee>> CreateEmployee(
        [FromBody] CreateEmployeeCommand command,
        CancellationToken cancellationToken)
    {
        var employee = await _createEmployeeCommandHandler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
    }
}
