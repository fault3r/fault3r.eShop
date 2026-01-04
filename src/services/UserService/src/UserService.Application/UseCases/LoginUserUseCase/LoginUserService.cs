
using System;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Domain.Common;
using UserService.Domain.Interfaces;

namespace UserService.Application.UseCases.LoginUserUseCase;

public sealed class LoginUserService(
    IUserDomainService userDomainService,
    ISessionService sessionService,
    ITokenService tokenService,
    ILogger<LoginUserService> logger
) : ILoginUserService
{
    private const int SessionLifetimeDays = 3;
    private readonly IUserDomainService _userDomainService = userDomainService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<LoginUserService> _logger = logger;

    public async Task<Result<LoginUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        _logger.LogInformation("Loginning user with '{Identity}' identity…", identity.Trim());

        var user = await _userDomainService.VerifyCredentialAsync(identity, password, ct);
        if (user is null)
        {
            _logger.LogWarning("Login failed: identity or password is incorrect!");

            return Result<LoginUserResult>.Failure("Identity or password is incorrect!");
        }

        var sessionId = Guid.NewGuid().ToString("N");

        var refreshToken = CryptRefreshToken.Generate();
        var refreshTokenHash = CryptRefreshToken.Hash(refreshToken);

        var now = DateTimeOffset.UtcNow;

        var session = new SessionData
        {
            SessionId = sessionId,
            DeviceId = "unknown", //
            IpAddress = "unknown", //
            CreatedAt = now,

            RefreshTokenHash = refreshTokenHash,
            RefreshTokenExpiresAt = now.AddDays(SessionLifetimeDays),  
            LastAccessedAt = now,

            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            Role = user.Role,
            Status = user.Status,
        };

        await _sessionService.CreateAsync(session, ct);

        var accessToken = await  _tokenService.GenerateAsync(sessionId, user.Id);

        _logger.LogInformation("User successfully logged in with '{Id}' identity.", user.Id.ToString());

        return Result<LoginUserResult>.Success(
            new LoginUserResult(accessToken, refreshToken));
    }
}
