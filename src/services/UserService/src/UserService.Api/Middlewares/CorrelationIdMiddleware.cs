
using System;
using Serilog.Context;

namespace UserService.Api.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private const string HeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request
            .Headers[HeaderName].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

        context.Items[HeaderName] = correlationId;
        
        context.Response.Headers[HeaderName] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<CorrelationIdMiddleware>();
}