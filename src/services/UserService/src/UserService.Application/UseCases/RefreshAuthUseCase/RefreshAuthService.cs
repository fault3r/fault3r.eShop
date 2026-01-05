
using System;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

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
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);

        var principal = await _tokenService.ReadClaimsAsync(accessToken);
        if (principal is null)
        {
            _logger.LogWarning("RefreshAuth failed: invalid access token!");

            return Result<RefreshAuthResult>.Failure("Invalid access token!");
        }

        var userId = principal.FindFirst("sub")?.Value;
        var sessionId = principal.FindFirst("jti")?.Value;

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sessionId))
        {
            _logger.LogWarning("RefreshAuth failed: missing token claims (sub or jti)!");

            return Result<RefreshAuthResult>.Failure("Invalid token claims!");
        }

        var session = await _sessionService.GetAsync(sessionId, ct);
        if (session is null)
        {
            _logger.LogInformation("RefreshAuth failed: session expired or invalidated for user '{UserId}', session '{SessionId}'", userId, sessionId);
            return Result<RefreshAuthResult>.Failure("Session expired or invalidated!");
        }

        var valid = CryptRefreshToken.Verify(refreshToken, session.RefreshTokenHash);
        if (!valid)
        {
            _logger.LogWarning("RefreshAuth failed: refresh token mismatch for user '{UserId}', session '{SessionId}'. Invalidating all sessions.", userId, sessionId);

            // -session hijacking!
            await _sessionService.InvalidateAllAsync(userId, ct);

            return Result<RefreshAuthResult>.Failure("Invalid refresh token!");
        }

        var newRefreshToken = CryptRefreshToken.Generate();
        var newRefreshTokenHash = CryptRefreshToken.Hash(newRefreshToken);
        var now = DateTimeOffset.UtcNow;

        session.RefreshTokenHash = newRefreshTokenHash;
        session.RefreshTokenExpiresAt = now.AddDays(SessionLifetimeDays);
        session.LastAccessedAt = now;

        await _sessionService.UpdateAsync(session, ct);

        var newAccessToken = await _tokenService.GenerateAsync(sessionId, userId);

        _logger.LogInformation( "User authentication successfully refreshed for user '{UserId}', session '{SessionId}'.", userId, sessionId );

        return Result<RefreshAuthResult>.Success(
            new RefreshAuthResult(newAccessToken, newRefreshToken)
        );
    }
}
