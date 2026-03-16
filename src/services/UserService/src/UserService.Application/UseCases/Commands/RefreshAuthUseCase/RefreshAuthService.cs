
using System;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Security.Authentication;

namespace UserService.Application.UseCases.Commands.RefreshAuthUseCase;

public sealed class RefreshAuthService(
    ITokenService tokenService,
    ISessionService sessionService,
    ILogger<RefreshAuthService> logger
) : IRefreshAuthService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISessionService _sessionService = sessionService;
    private readonly ILogger<RefreshAuthService> _logger = logger;
    private const int SessionLifetimeDays = 3;

    public async Task<Result<RefreshAuthResult>> ExecuteAsync(
        string accessToken,
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        _logger.LogInformation("Refreshing authentication…");

        var claims = _tokenService.ReadAccessTokenClaims(accessToken);
        if (claims is null)
        {
            _logger.LogWarning("Invalid access token!");

            return Result<RefreshAuthResult>.Failure("Invalid access token!");
        }

        var userId = claims.FindFirst("sub")?.Value;
        var sessionId = claims.FindFirst("jti")?.Value;

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sessionId))
        {
            _logger.LogWarning("Invalid token claims!");

            return Result<RefreshAuthResult>.Failure("Invalid token claims!");
        }

        _logger.LogInformation("Refreshing authentication for {User}…", userId);

        var session = await _sessionService.GetAsync(sessionId, cancellationToken);
        if (session is null)
        {
            _logger.LogWarning("Session {Session} expired or invalidated.", sessionId);

            return Result<RefreshAuthResult>.Failure("Session expired or invalidated!");
        }

        var valid = _tokenService.VerifyRefreshToken(refreshToken, session.RefreshTokenHash);
        if (!valid)
        {
            // ⟶session hijacking!
            await _sessionService.InvalidateAllAsync(userId, cancellationToken);

            _logger.LogWarning("Refresh token mismatch for session {Session}, All user sessions invalidated!", sessionId);

            return Result<RefreshAuthResult>.Failure("Invalid refresh token!");
        }

        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var newRefreshTokenHash = _tokenService.ComputeRefreshTokenHash(newRefreshToken);
        var now = DateTimeOffset.UtcNow;

        session.RefreshTokenHash = newRefreshTokenHash;
        session.RefreshTokenExpiresAt = now.AddDays(SessionLifetimeDays);
        session.LastAccessedAt = now;

        await _sessionService.UpdateAsync(session, cancellationToken);

        var newAccessToken = _tokenService.GenerateAccessToken(sessionId, userId);

        _logger.LogInformation("Athentication successfully refreshed for user {UserId} with {SessionId} session.", userId, sessionId);

        return Result<RefreshAuthResult>.Success(
            new RefreshAuthResult(newAccessToken, newRefreshToken)
        );
    }
}
