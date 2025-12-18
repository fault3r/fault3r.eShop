
using System;
using Serilog;
using Serilog.Context;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Middlewares;

public class CrossCuttingMiddleware
{

    private readonly RequestDelegate _next;
    private readonly string correlationHeader;

    public CrossCuttingMiddleware(
        RequestDelegate next,
        string correlationHeader)
    {
        _next = next;
        this.correlationHeader = correlationHeader;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ICorrelationContext correlationContext)
    {
        if (correlationContext is null)
            throw new ArgumentNullException(nameof(correlationContext));

        var correlationId = context.Request.Headers[correlationHeader]
            .FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        correlationContext.Set(correlationId);

        context.Items[correlationHeader] = correlationId;
        context.Response.Headers[correlationHeader] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            Log.Information(
                "Incoming request {Method} {Path}.", context.Request.Method, context.Request.Path);

            await _next(context);

            Log.Information(
                "Completed request {Method} {Path} with status {StatusCode}.", context.Request.Method, context.Request.Path, context.Response.StatusCode);
        }
    }
}

public static class CrossCuttingMiddlewareExtensions
{
    public static IApplicationBuilder UseCrossCuttingMiddleware(
        this IApplicationBuilder builder,
        string correlationHeader
    )
       => builder.UseMiddleware<CrossCuttingMiddleware>(correlationHeader);
}

