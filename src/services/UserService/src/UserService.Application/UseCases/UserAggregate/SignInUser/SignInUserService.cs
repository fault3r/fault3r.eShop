
using System;
using System.Security.Cryptography;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;

namespace UserService.Application.UseCases.UserAggregate.SignInUser;

public sealed class SignInUserService : ISignInUserService
{
    private readonly IUserDomainService _userDomainService;
    private readonly ISessionService _sessionService;
    private readonly ITokenService _tokenService;

    public SignInUserService(
        IUserDomainService userDomainService,
        ISessionService sessionService,
        ITokenService tokenService)
    {
        _userDomainService = userDomainService;
        _sessionService = sessionService;
        _tokenService = tokenService;
    }

    public async Task<Result<SignInUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate user credentials
        var user = await _userDomainService.VerifyCredentialAsync(identity, password, cancellationToken);
        if (user is null)
            return Result<SignInUserResult>.Failure("Invalid credentials");

        // 2. Generate sessionId
        var sessionId = Guid.NewGuid().ToString("N");

        // 3. Generate refresh token
        var refreshToken = CryptRefreshToken.GenerateToken();
        var refreshTokenHash = CryptRefreshToken.ToBCryptHash(refreshToken);

        // 4. Build session data
        var session = new SessionData
        {
            SessionId = sessionId,
            DeviceId = "unknown", // or from request
            IpAddress = "unknown", // or from request
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

        // 5. Store session in Redis
        await _sessionService.CreateSessionAsync(session, cancellationToken);

        // 6. Generate access token
        var accessToken = _tokenService.GenerateAccessToken(sessionId, user.Id);

        // 7. Return both tokens
        return Result<SignInUserResult>.Success(new SignInUserResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

}
