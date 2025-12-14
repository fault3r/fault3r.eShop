
using System;
using Serilog;
using Serilog.Context;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string header = "X-Correlation-ID";

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(
        HttpContext context, ICorrelationContext correlationContext)
    {
        var correlationId = context.Request.Headers[header]
            .FirstOrDefault() ?? Guid.NewGuid().ToString();

        correlationContext.Set(correlationId);

        context.Items[header] = correlationId;
        context.Response.Headers[header] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {            
            Log.Information(
                "Incoming request {Method} {Path}.", context.Request.Method, context.Request.Path);               

            try
            {
                await _next(context);

                Log.Information(
                    "Completed request {Method} {Path} with status {StatusCode} and correlation id {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    correlationId);
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Unhandled exception for {Method} {Path} with correlation id {CorrelationId}",
                    context.Request.Method,
                    context.Request.Path,
                    correlationId);

                throw;
            }
        }
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(
        this IApplicationBuilder builder)
            => builder.UseMiddleware<CorrelationIdMiddleware>();
}

