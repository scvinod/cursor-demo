using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace cursor_demo_api.Middleware;

public sealed class ExceptionLoggingHandler : IExceptionHandler
{
    private readonly ILogger<ExceptionLoggingHandler> _logger;

    public ExceptionLoggingHandler(ILogger<ExceptionLoggingHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            $"Unhandled exception processing {httpContext.Request.Method} {httpContext.Request.Path}.");

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "The request could not be completed. See application logs for details."
            },
            cancellationToken);

        return true;
    }
}
