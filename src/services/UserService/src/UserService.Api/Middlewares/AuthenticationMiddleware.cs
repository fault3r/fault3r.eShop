
using System;
using System.IdentityModel.Tokens.Jwt;
using UserService.Application.Interfaces;

namespace UserService.Api.Middlewares;

public class AuthenticationMiddleware(
    RequestDelegate next,
    ITokenService tokenService,
    ISessionService sessionService)
{
    private readonly RequestDelegate _next = next;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISessionService _sessionService = sessionService;

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await _next(context);
            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        var principal = await _tokenService.ReadClaimsAsync(token);
        if (principal is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var sessionId = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var exists = await _sessionService.SessionExistAsync(sessionId);
        if (!exists)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        context.User = principal;

        await _next(context);
    }
}

public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder builder
    )
        => builder.UseMiddleware<AuthenticationMiddleware>();
}
