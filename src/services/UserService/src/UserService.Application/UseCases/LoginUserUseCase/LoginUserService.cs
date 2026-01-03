
using System;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;

namespace UserService.Application.UseCases.LoginUserUseCase;

public sealed class LoginUserService(
    IUserDomainService userDomainService,
    ISessionService sessionService,
    ITokenService tokenService
) : ILoginUserService
{
    private readonly IUserDomainService _userDomainService = userDomainService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<LoginUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        var user = await _userDomainService.VerifyCredentialAsync(identity, password, ct);
        if (user is null)
            return Result<LoginUserResult>.Failure("Invalid credentials");

        var sessionId = Guid.NewGuid().ToString("N");

        var refreshToken = CryptRefreshToken.Generate();
        var refreshTokenHash = CryptRefreshToken.Hash(refreshToken);

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

        await _sessionService.CreateSessionAsync(session, ct);

        var accessToken = await  _tokenService.GenerateAccessTokenAsync(sessionId, user.Id);

        return Result<LoginUserResult>.Success(new LoginUserResult(accessToken, refreshToken));
    }
}
