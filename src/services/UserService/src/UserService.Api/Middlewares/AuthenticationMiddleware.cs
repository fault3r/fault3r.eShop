
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

    public async Task Invoke(HttpContext context)
    {

        var accessToken = context.Request.Headers["Authorization"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();

        var refreshToken = context.Request.Headers["X-Refresh-Token"]
            .FirstOrDefault();

        string? sessionId = null;

        var principal = _tokenService.ValidateAccessToken(accessToken);
        if (principal != null)
        {
            sessionId = principal.FindFirst("sessionId")?.Value;
        }

        string? sessionKey = sessionId ?? refreshToken;

        if (!string.IsNullOrWhiteSpace(sessionKey))
        {
            var session = await _sessionService.GetSessionAsync(sessionKey);

            if (session != null)
            {
                using var scope = _scopeFactory.CreateScope();
                var userService = scope.ServiceProvider.GetRequiredService<IUserDomainService>();
                var user = await userService.GetUserByIdAsync(session.UserId);

                var newAccessToken = _tokenService.GenerateAccessToken(
                    session.SessionId,
                    session.UserId
                );

                context.Response.Headers["x-new-access-token"] = newAccessToken;
                context.User = _tokenService.ValidateAccessToken(newAccessToken)!;
            }
        }
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
