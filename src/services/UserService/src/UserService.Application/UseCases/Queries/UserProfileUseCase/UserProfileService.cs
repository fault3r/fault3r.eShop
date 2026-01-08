
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
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        _logger.LogInformation("Fetching profile from '{SessionId}' session…", sessionId.Trim());

        var session = await _sessionService.GetAsync(sessionId, cancellationToken);
        if (session is null)
        {
            _logger.LogWarning("Session expired or invalidated!");

            return Result<UserProfileResult>.Failure("Session expired or invalidated!");
        }

        _logger.LogInformation("profile successfully retrieved for '{UserId}' user.", session.UserId);

        return Result<UserProfileResult>.Success(new UserProfileResult(
            session.UserId,
            session.Email,
            session.FullName,
            session.Role,
            session.Status
        ));
    }
}
