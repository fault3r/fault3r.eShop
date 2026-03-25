
using System;
using System.Text.Json;
using UserService.Application.CrossCutting;
using UserService.Domain.Security.Authentication;

namespace UserService.Api.Middlewares;

public sealed class AuthenticationMiddleware(
    RequestDelegate next,
    ITokenService tokenService,
    ISessionService sessionService,
    IJsonSerializer jsonSerializer)
{
    private readonly RequestDelegate _next = next;
    private readonly ITokenService _token = tokenService;
    private readonly ISessionService _session = sessionService;
    private readonly IJsonSerializer _serializer = jsonSerializer;
    private const string TokenPrefix = "Bearer ";

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            await _next(context);
            return;
        }

        if (!authHeader.StartsWith(TokenPrefix))
        {
            await WriteResponseErrorAsync(context, "Invalid authorization!", context.RequestAborted);
            return;
        }

        var token = authHeader[TokenPrefix.Length..].Trim();

        var claims = _token.ReadAccessTokenClaims(token);

        if (claims is null)
        {
            await WriteResponseErrorAsync(context, "Invalid token!", context.RequestAborted);
            return;
        }

        var sessionId = claims.FindFirst("jti")?.Value;

        if (string.IsNullOrWhiteSpace(sessionId))
        {
            await WriteResponseErrorAsync(context, "Invalid token claims!", context.RequestAborted);
            return;
        }

        var validate = await _session.ExistsAsync(sessionId, context.RequestAborted);

        if (!validate)
        {
            await WriteResponseErrorAsync(context, "Invalidated session!", context.RequestAborted);
            return;
        }

        context.User = claims;
        await _next(context);
    }

    private async Task WriteResponseErrorAsync(
        HttpContext context,
        string errorMessage,
        CancellationToken cancellationToken)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        var responseBody = JsonSerializer.Serialize(new { errorMessage }, _serializer.DefaultOptions);

        await context.Response.WriteAsync(responseBody, cancellationToken);
    }
}

public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder builder)
    => builder.UseMiddleware<AuthenticationMiddleware>();
}
