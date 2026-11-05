namespace cursor_demo_api.Models;

public sealed class Employee
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int Age { get; init; }

    public decimal Salary { get; init; }

    public string PhoneNumber { get; init; } = string.Empty;
}
