
using System;
using UserService.Application.Interfaces;
using UserService.Application.Security.Authentication;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.RefreshAuthUseCase;

public sealed class RefreshAuthService(
    ITokenService tokenService,
    ISessionService sessionService) : IRefreshAuthService
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly ISessionService _sessionService = sessionService;

    public async Task<Result<RefreshAuthResult>> ExecuteAsync(
        string expiredAccessToken,
        string providedRefreshToken,
        CancellationToken cancellationToken = default)
    {
        var principal = _tokenService.ReadPrincipal(expiredAccessToken)
            ?? throw new UnauthorizedAccessException("Invalid access token");
        
        var userId = principal.FindFirst("sub")?.Value;
        var sessionId = principal.FindFirst("jti")?.Value;

        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(sessionId))
            throw new UnauthorizedAccessException("Invalid token claims");

        var session = await _sessionService.GetSessionAsync(sessionId, cancellationToken)
            ?? throw new UnauthorizedAccessException("Session expired or invalidated");

        var valid = CryptRefreshToken.Verify(providedRefreshToken, session.RefreshTokenHash);
        if (!valid)
        {
            await _sessionService.InvalidateAllUserSessionsAsync(userId, cancellationToken);
            throw new UnauthorizedAccessException("Refresh token invalid");
        }

        var newRefreshToken = CryptRefreshToken.Generate();
        var newRefreshTokenHash = CryptRefreshToken.ToHash(newRefreshToken);

        session.RefreshTokenHash = newRefreshTokenHash;
        session.RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(30);
        session.LastAccessedAt = DateTimeOffset.UtcNow;

        await _sessionService.UpdateSessionAsync(session, cancellationToken);

        var newAccessToken = _tokenService.GenerateAccessToken(userId, sessionId);

         return Result<RefreshAuthResult>.Success(new RefreshAuthResult(newAccessToken, newRefreshToken));
    }
}
