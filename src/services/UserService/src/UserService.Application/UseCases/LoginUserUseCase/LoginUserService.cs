
using System;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;

namespace UserService.Application.UseCases.LoginUserUseCase;

public sealed class LoginUserService(
    IUserDomainService userDomainService,
    ISessionService sessionService,
    ITokenService tokenService) : ILoginUserService
{
    private readonly IUserDomainService _userDomainService = userDomainService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<LoginUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await _userDomainService.VerifyCredentialAsync(identity, password, cancellationToken);
        if (user is null)
            return Result<LoginUserResult>.Failure("Invalid credentials");

        var sessionId = Guid.NewGuid().ToString("N");

        var refreshToken = CryptRefreshToken.Generate();
        var refreshTokenHash = CryptRefreshToken.ToHash(refreshToken);

        var session = new SessionData
        {
            SessionId = sessionId,
            DeviceId = "unknown",
            IpAddress = "unknown",
            CreatedAt = DateTimeOffset.UtcNow,
            LastAccessedAt = DateTimeOffset.UtcNow,

            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            Status = user.Status,

            RefreshTokenHash = refreshTokenHash,
            RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(30) // settings
        };

        await _sessionService.CreateSessionAsync(session, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(sessionId, user.Id);

        return Result<LoginUserResult>.Success(new LoginUserResult(accessToken, refreshToken));
    }
}
