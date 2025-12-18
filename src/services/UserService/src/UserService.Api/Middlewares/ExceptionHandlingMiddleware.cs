
using System;
using Serilog;

namespace UserService.Api.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Log.Error(
                exception,
                "Unhandled exception occurred while processing {Method} {Path}", context.Request.Method, context.Request.Path
            );

            var response = new
            {
                error = "Internal Server Error",
                correlationId = context.Items["X-Correlation-ID"],
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlingMiddleware>();
}
