
using System;
using Serilog;
using Serilog.Context;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Middlewares;

public class CrossCuttingMiddleware(RequestDelegate next)
{
    private const string header = "X-Correlation-ID";

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(
        HttpContext context, ICorrelationContext correlationContext)
    {
        var correlationId = context.Request.Headers[header]
            .FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        correlationContext.Set(correlationId);
        context.Items[header] = correlationId;
        context.Response.Headers[header] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {            
            Log.Information(
                "Incoming request {Method} {Path}", context.Request.Method, context.Request.Path);   

            await _next(context);

            Log.Information(
                "Completed request {Method} {Path} with status {StatusCode}", context.Request.Method, context.Request.Path, context.Response.StatusCode);       
        }
    }
}

public static class CrossCuttingMiddlewareExtensions
{
    public static IApplicationBuilder UseCrossCuttingMiddleware(
        this IApplicationBuilder builder)
            => builder.UseMiddleware<CrossCuttingMiddleware>();
}

