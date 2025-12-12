
using System;
using Serilog.Context;
using UserService.Infrastructure.CrossCutting;

namespace UserService.Api.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    private const string header = "X-Correlation-ID";

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
            await _next(context);
        }
    }
}

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationIdMiddleware(
        this IApplicationBuilder builder)
            => builder.UseMiddleware<CorrelationIdMiddleware>();
}