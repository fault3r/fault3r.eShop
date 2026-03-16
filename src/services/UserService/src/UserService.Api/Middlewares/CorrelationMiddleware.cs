
using System;
using Serilog;
using Serilog.Context;
using UserService.Application.CrossCutting;

namespace UserService.Api.Middlewares;

public sealed class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string correlationHeader;

    public CorrelationMiddleware(
        RequestDelegate next,
        string correlationHeader)
    {
        ArgumentException.ThrowIfNullOrEmpty(correlationHeader);

        _next = next;
        this.correlationHeader = correlationHeader;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ICorrelationContext correlationContext)
    {
        var correlationId = context.Request.Headers[correlationHeader]
            .FirstOrDefault()
        ?? Guid.NewGuid().ToString();

        correlationContext.Set(correlationId);

        context.Items[correlationHeader] = correlationId;
        context.Response.Headers[correlationHeader] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            Log.Information("Incoming request {Method} {Path}.",
                context.Request.Method, context.Request.Path);

            await _next(context);

            Log.Information("Completed request {Method} {Path} with status code {StatusCode}.",
                context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }
}

public static class CorrelationMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationMiddleware(
        this IApplicationBuilder builder,
        string correlationHeader)
    => builder.UseMiddleware<CorrelationMiddleware>(correlationHeader);
}

