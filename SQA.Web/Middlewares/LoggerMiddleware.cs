using System.Net;
using Integrated.Loggers;

namespace SQA.Web.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ICustomLogger _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        List<ScopeContextItem> scopeContext = new();
        scopeContext.Add(new("Time", DateTime.Now.ToString()));

        using (var scope = _logger.BeginScope(scopeContext))
        {
            try
            {
                await _next(context);

                _logger.LogInformation("Finished Processing the request succesfully");
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                _logger.LogError(ex);

                await context.Response.WriteAsync("Internal Server Error");
            }
        }
    }

    public LoggerMiddleware(RequestDelegate next, ICustomLogger logger)
    {
        _next = next;
        _logger = logger;
    }
}