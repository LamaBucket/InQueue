using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SQA.Web.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            await context.Response.WriteAsync("Internal Server Error");
        }
    }

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
}