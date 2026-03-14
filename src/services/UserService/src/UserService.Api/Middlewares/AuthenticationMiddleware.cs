
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
            await WriteResponseErrorAsync(context, "Invalid authorization!");
            return;
        }
        var token = authHeader[TokenPrefix.Length..].Trim();

        var claims = _token.ReadAccessTokenClaims(token);
        if (claims is null)
        {
            await WriteResponseErrorAsync(context, "Invalid token!");
            return;
        }

        var sessionId = claims.FindFirst("jti")?.Value;
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            await WriteResponseErrorAsync(context, "Invalid token claims!");
            return;
        }

        var session = await _session.ExistsAsync(sessionId, context.RequestAborted);
        if (!session)
        {
            await WriteResponseErrorAsync(context, "Invalidated session!");
            return;
        }

        context.User = claims;
        await _next(context);
    }

    private async Task WriteResponseErrorAsync(
        HttpContext context, string errorMessage)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var errorResponse = new { errorMessage };
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(errorResponse, _serializer.Options));
    }
}

public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder builder)
    => builder.UseMiddleware<AuthenticationMiddleware>();
}
