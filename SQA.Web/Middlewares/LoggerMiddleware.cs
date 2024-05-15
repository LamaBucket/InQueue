using System.Net;
using Integrated.Loggers;
using Microsoft.AspNetCore.Http.Extensions;
using SQA.Domain;
using SQA.Domain.Exceptions;

namespace SQA.Web.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;

    private readonly ILogger<LoggerMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            string client = context.User?.Identity?.Name ?? context.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? string.Empty;
            string url = context.Request.GetEncodedPathAndQuery().ToString();
            string method = context.Request.Method.ToString();

            _logger.LogInformation("Caught Request From Client. Time: {Time}, Client: {client}, Query: {query} Method: {method}", DateTime.Now, client, url, method);

            await _next(context);

            _logger.LogInformation("Finished Processing Client Request Succesfully");
        }
        catch (DomainException domainEx)
        {
            string message = domainEx.Message; //Create Exception Message logic

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            await context.Response.WriteAsync(message);
        }
        catch (Exception ex)
        {
            string message = ex.ToString();

            _logger.LogError("Server caught an exception: {message}", message);


            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync("Internal Server Error");
        }
    }

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
}