
using System;
using Serilog;

namespace UserService.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string correlationHeader;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        string correlationHeader)
    {
        ArgumentException.ThrowIfNullOrEmpty(correlationHeader);

        _next = next;
        this.correlationHeader = correlationHeader;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            Log.Error(
                exception, "Unhandled exception occurred while processing {Method} {Path}.", context.Request.Method, context.Request.Path);

            context.Response.ContentType = "application/json";
            var response = new
            {
                error = "Internal Server Error",
                correlationId = context.Items[correlationHeader],
            };
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder,
        string correlationHeader
    )
        => builder.UseMiddleware<ExceptionHandlingMiddleware>(correlationHeader);
}
