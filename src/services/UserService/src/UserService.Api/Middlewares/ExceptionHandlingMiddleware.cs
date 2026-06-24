
using System;
using System.Text.Json;
using Serilog;
using UserService.Application.CrossCutting;

namespace UserService.Api.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string correlationHeader;
    private readonly IJsonSerializer _serializer;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        string correlationHeader,
        IJsonSerializer jsonSerializer)
    {
        ArgumentException.ThrowIfNullOrEmpty(correlationHeader);

        _next = next;
        this.correlationHeader = correlationHeader;
        _serializer = jsonSerializer;
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
        catch (Exception ex)
        {
            Log.Error(ex, "An unhandled exception occurred!");

            var response = JsonSerializer.Serialize(new
            {
                errorMessage = "Internal Server Error",
                correlationId = context.Items[correlationHeader],
            }, _serializer.DefaultOptions);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response, context.RequestAborted);
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder,
        string correlationHeader)
    => builder.UseMiddleware<ExceptionHandlingMiddleware>(correlationHeader);
}