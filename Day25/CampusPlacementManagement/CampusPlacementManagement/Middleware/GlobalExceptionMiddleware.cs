
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "An unexpected error occurred while processing the request.");

            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var statusCode = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,

            UnauthorizedAccessException =>
                StatusCodes.Status403Forbidden,

            InvalidOperationException =>
                StatusCodes.Status409Conflict,

            ArgumentException =>
                StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status500InternalServerError
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected server error occurred."
                : exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails);
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            _ => "Internal Server Error"
        };
    }
}
