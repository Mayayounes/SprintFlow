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
        Console.WriteLine("Middleware entered");

        try
        {
            await _next(context);
        }
        catch (ConcurrencyException ex)
        {
            Console.WriteLine("Concurrency middleware hit");

            context.Response.StatusCode = StatusCodes.Status409Conflict;

            var response = Result<string>.Failure(
                new List<string> { ex.Message },
                "ConcurrencyConflict"
            );

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = Result<string>.Failure(
                new List<string> { ex.Message },
                "Unexpected error"
            );

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}