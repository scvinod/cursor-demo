using System.ComponentModel.DataAnnotations;
using cursor_demo_api.Models;

namespace cursor_demo_api.Application.Employees.Commands;

public sealed class CreateEmployeeCommand
{
    [Required]
    [MaxLength(EmployeeConstraints.NameMaxLength)]
    public string Name { get; init; } = string.Empty;

    [Range(EmployeeConstraints.MinAge, EmployeeConstraints.MaxAge)]
    public int Age { get; init; }

    [Range(typeof(decimal), "0", "999999999")]
    public decimal Salary { get; init; }

    [Required]
    [MaxLength(EmployeeConstraints.PhoneNumberMaxLength)]
    [Phone]
    public string PhoneNumber { get; init; } = string.Empty;
}
