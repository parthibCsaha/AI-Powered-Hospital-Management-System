

using System.Diagnostics;

namespace HMS_Backend.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;


    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
 
        _logger.LogInformation(
            "HTTP {Method} {Path} started. TraceId: {TraceId}",
            context.Request.Method,
            context.Request.Path,
            context.TraceIdentifier);
 
        await _next(context);
 
        sw.Stop();
 
        _logger.LogInformation(
            "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms. TraceId: {TraceId}",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            sw.ElapsedMilliseconds,
            context.TraceIdentifier);
    }

}