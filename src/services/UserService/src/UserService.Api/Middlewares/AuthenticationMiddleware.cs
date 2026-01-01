using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;

namespace UserService.Api.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenService _tokenService;
    private readonly ISessionService _sessionService;
    private readonly IUserDomainService _userService;

    public AuthenticationMiddleware(
        RequestDelegate next,
        ITokenService tokenService,
        ISessionService sessionService,
        IUserDomainService userService)
    {
        _next = next;
        _tokenService = tokenService;
        _sessionService = sessionService;
        _userService = userService;
    }

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
            var user = await _userService.GetUserByIdAsync(session.UserId);

            var newAccessToken = _tokenService.GenerateAccessToken(
                session.SessionId,
                session.UserId
            );

            context.Response.Headers["x-new-access-token"] = newAccessToken;
            context.User = _tokenService.ValidateAccessToken(newAccessToken)!;

            await _next(context);
            return;
        }
    }

    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
}

}
public static class AuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthenticationMiddleware(
        this IApplicationBuilder builder
    )
        => builder.UseMiddleware<AuthenticationMiddleware>();
}
