
using System;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Security.Authentication;

namespace UserService.Application.UseCases.Commands.LoginUserUseCase;

public sealed class LoginUserService(
    IUserDomainService userDomainService,
    ISessionService sessionService,
    ITokenService tokenService,
    ILogger<LoginUserService> logger
) : ILoginUserService
{
    private const int SessionLifetimeDays = 3;
    private readonly IUserDomainService _userService = userDomainService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly ILogger<LoginUserService> _logger = logger;

    public async Task<Result<LoginUserResult>> ExecuteAsync(
        string identity,
        string password,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(identity);
        ArgumentException.ThrowIfNullOrWhiteSpace(password);

        _logger.LogInformation("Loginning user with '{Identity}' identity…", identity.Trim());

        var verified = await _userService.VerifyCredentialsAsync(identity, password, cancellationToken);
        if (verified.IsFailure)
        {
            _logger.LogWarning("Invalid credentials!");

            return Result<LoginUserResult>.Failure("Invalid credentials!");
        }

        var user = verified.Value!;

        if (user.Status.IsLocked)
        {
            _logger.LogWarning("User is locked!");

            return Result<LoginUserResult>.Failure("User is locked!");
        }

        var sessionId = Guid.NewGuid().ToString("N");

        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenHash = _tokenService.ComputeRefreshTokenHash(refreshToken);

        var now = DateTimeOffset.UtcNow;

        var session = new SessionData
        {
            SessionId = sessionId,
            DeviceId = "unknown",
            IpAddress = "unknown",
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

        await _sessionService.CreateAsync(session, cancellationToken);

        var accessToken = _tokenService.GenerateAccessToken(sessionId, user.Id);

        _logger.LogInformation("User successfully logged in with '{SessionId}' session.", sessionId);

        return Result<LoginUserResult>.Success(
            new LoginUserResult(accessToken, refreshToken));
    }
}
