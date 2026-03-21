


using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using HMS_Backend.Application.Common;


namespace HMS_Backend.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    public async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, response) = ex switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                ApiResponse.Fail(
                    "Validation failed.",
                    new List<string> { validationEx.Message }
                )
            ),
            KeyNotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                ApiResponse.Fail(notFoundEx.Message)
            ),
            UnauthorizedAccessException unauthorizedEx => (
                HttpStatusCode.Unauthorized,
                ApiResponse.Fail(unauthorizedEx.Message)
            ),
            InvalidOperationException invalidOpEx => (
                HttpStatusCode.Conflict,
                ApiResponse.Fail(invalidOpEx.Message)
            ),
            ArgumentException argEx => (
                HttpStatusCode.BadRequest,
                ApiResponse.Fail(argEx.Message)
            ),
            _ => (
                HttpStatusCode.InternalServerError,
                ApiResponse.Fail("An unexpected error occurred. Please try again later.")
            )   
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(ex, "An unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
        }
        else
        {
            _logger.LogWarning("Handled {StatusCode} exception: {Message}. TraceId: {TraceId}", (int)statusCode, ex.Message, context.TraceIdentifier);
        }

        context.Response.StatusCode = (int)statusCode;

        var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));

    }

}