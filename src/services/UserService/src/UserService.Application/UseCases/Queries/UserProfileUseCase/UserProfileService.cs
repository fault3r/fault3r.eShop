
using System;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.Queries.UserProfileUseCase;

public sealed class UserProfileService(
    ISessionService sessionService,
    ILogger<UserProfileService> logger
) : IUserProfileService
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly ILogger<UserProfileService> _logger = logger;

    public async Task<Result<UserProfileResult>> ExecuteAsync(
        string sessionId,
        CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        _logger.LogInformation("Fetching profile from session '{SessionId}'…", sessionId);

        var session = await _sessionService.GetAsync(sessionId, ct);
        if (session is null)
        {
            _logger.LogWarning("Session '{SessionId}' not found or expired!", sessionId);

            return Result<UserProfileResult>.Failure("Session not found or expired!");
        }

        _logger.LogInformation("User profile successfully retrieved for user '{UserId}', session '{SessionId}'.",
            session.UserId, session.SessionId);

        return Result<UserProfileResult>.Success(new UserProfileResult(
            session.UserId,
            session.Email,
            session.FullName,
            session.Role,
            session.Status
        ));
    }
}
