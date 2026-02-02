using EventSourcingSample.ROP;
using EventSourcingSample.WebAPI.ROP;
using Microsoft.AspNetCore.Diagnostics;

namespace EventSourcingSample.WebAPI.Exceptions;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.ToString());

        var error = Result.Failure("Server error");

        httpContext.Response.StatusCode = (int)error.HttpStatusCode;

        await httpContext.Response.WriteAsJsonAsync(error.ToDto(), cancellationToken);

        return true;
    }
}
