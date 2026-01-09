
using System;
using UserService.Application.Interfaces;

namespace UserService.Api.Middlewares;

public sealed class AuthenticationMiddleware(
    RequestDelegate next,
    ITokenService tokenService,
    ISessionService sessionService)
{
    private readonly RequestDelegate _next = next;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISessionService _sessionService = sessionService;
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
            await WriteResponseError(ref context, "Invalid authorization!");
            return;
        }
        var token = authHeader[TokenPrefix.Length..].Trim();

        var claims = await _tokenService.ReadClaimsAsync(token);
        if (claims is null)
        {
            await WriteResponseError(ref context, "Invalid token!");
            return;
        }

        var sessionId = claims.FindFirst("jti")?.Value;
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            await WriteResponseError(ref context, "Invalid token claims!");
            return;
        }

        var session = await _sessionService.ExistAsync(sessionId);
        if (!session)
        {
            await WriteResponseError(ref context, "Invalidated session!");
            return;
        }

        context.User = claims;
        await _next(context);
    }

    private static Task WriteResponseError(ref HttpContext context, string errorMessage)
    {
        var response = new { errorMessage };

        context.Response.ContentType = "application/json";
        context.Response.WriteAsJsonAsync(response);
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

        return Task.CompletedTask;
    }
}

public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder builder
    )
        => builder.UseMiddleware<AuthenticationMiddleware>();
}
