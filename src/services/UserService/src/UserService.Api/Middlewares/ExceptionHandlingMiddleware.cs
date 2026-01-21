
using System;
using Serilog;

namespace UserService.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware
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
        catch (OperationCanceledException)
        {
            Log.Warning("Operation was canceled!");
            throw;
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Unhandled exception occurred!");

            var response = new
            {
                errorMessage = "Internal Server Error",
                correlationId = context.Items[correlationHeader],
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(response);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
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
