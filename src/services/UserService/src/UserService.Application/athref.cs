public sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly ISessionService _sessionService;
    private readonly ITokenService _tokenService;

    public RefreshTokenService(
        ISessionService sessionService,
        ITokenService tokenService)
    {
        _sessionService = sessionService;
        _tokenService = tokenService;
    }

    public async Task<Result<RefreshResult>> ExecuteAsync(
        string expiredAccessToken,
        string providedRefreshToken,
        CancellationToken cancellationToken = default)
    {
        // 1. Extract sessionId from expired access token
        var principal = _tokenService.ReadExpiredToken(expiredAccessToken);
        var sessionId = principal.FindFirst("sessionId")?.Value;

        if (string.IsNullOrWhiteSpace(sessionId))
            return Result.Fail<RefreshResult>("Invalid access token");

        // 2. Load session WITHOUT sliding expiration
        var session = await _sessionService.GetSessionWithoutSlidingAsync(sessionId, cancellationToken);
        if (session is null)
            return Result.Fail<RefreshResult>("Session not found");

        // 3. Validate refresh token hash
        if (!BCrypt.Net.BCrypt.Verify(providedRefreshToken, session.RefreshTokenHash))
        {
            // Token theft detected
            await _sessionService.InvalidateSessionAsync(sessionId, cancellationToken);
            return Result.Fail<RefreshResult>("Invalid refresh token");
        }

        // 4. Rotate refresh token
        var newRefreshToken = TokenGenerator.GenerateSecureRandom();
        var newRefreshHash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);

        session.RefreshTokenHash = newRefreshHash;
        session.RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(30);
        session.LastAccessedAt = DateTimeOffset.UtcNow;

        // 5. Update session (with sliding expiration)
        await _sessionService.UpdateSessionAsync(session, cancellationToken);

        // 6. Issue new access token
        var newAccessToken = _tokenService.GenerateAccessToken(session.SessionId, session.UserId);

        // 7. Return both tokens
        return Result.Ok(new RefreshResult(newAccessToken, newRefreshToken));
    }
}
