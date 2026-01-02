
using System;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Api.Middlewares;

public class AuthenticationMiddleware(
    RequestDelegate next,
    ITokenService tokenService,
    ISessionService sessionService,
    IServiceScopeFactory serviceScopeFactory)
{
    private readonly RequestDelegate _next = next;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;
   public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await _next(context);
            return;
        }

        var token = authHeader["Bearer ".Length..].Trim();

        // 1. Validate access token (must fail if expired)
        var principal = _tokenService.ValidateAccessToken(token);
        if (principal is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 2. Extract sessionId
        var sessionId = principal.FindFirst("sessionId")?.Value;
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 3. Check session exists in Redis (fast check)
        var exists = await _sessionService.SessionExistsAsync(sessionId);
        if (!exists)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // 4. Attach principal to HttpContext
        context.User = principal;

        await _next(context);
    }
public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder builder
    )
        => builder.UseMiddleware<AuthenticationMiddleware>();
}
