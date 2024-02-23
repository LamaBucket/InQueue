using System.Net;

namespace SQA.Web.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<LoggerMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            using (var scope = _logger.BeginScope("Scope1"))
            {
                _logger.LogInformation("Info1");
                await _next(context);
            }
            _logger.LogInformation("Info2");
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError(ex, "Critical Error");

            await context.Response.WriteAsync("Internal Server Error");
        }
    }

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
}