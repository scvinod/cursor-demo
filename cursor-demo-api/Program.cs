using cursor_demo_api.Application.Employees.Commands;
using cursor_demo_api.Application.Employees.Queries;
using cursor_demo_api.Cqrs;
using cursor_demo_api.Data;
using cursor_demo_api.Middleware;
using cursor_demo_api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionLoggingHandler>();
builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<EmployeeSchemaInitializer>();
builder.Services.AddScoped<ICommandHandler<CreateEmployeeCommand, Employee>, CreateEmployeeCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetEmployeesQuery, IReadOnlyList<Employee>>, GetEmployeesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetEmployeeByIdQuery, Employee?>, GetEmployeeByIdQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var schemaInitializer = scope.ServiceProvider.GetRequiredService<EmployeeSchemaInitializer>();
    await schemaInitializer.EnsureCreatedAsync(CancellationToken.None);
}

app.Run();
