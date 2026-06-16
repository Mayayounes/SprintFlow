using sprintFlow.Application.Common;
using sprintFlow.Application.Common.Exceptions;

namespace sprintFlow.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ConcurrencyException ex)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "application/json";

            var response = Result<object>.Failure(
                new List<string>
                {
                "This record was modified by another user. Refresh and try again."
                },
                "ConcurrencyConflict",
                ex.LatestState
            );

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = Result<string>.Failure(
                new List<string> { ex.Message },
                "Unexpected error"
            );

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}