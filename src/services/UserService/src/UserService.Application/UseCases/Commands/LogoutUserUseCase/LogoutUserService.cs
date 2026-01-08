using System;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Common;

namespace UserService.Application.UseCases.Commands.LogoutUserUseCase;

public class LogoutUserService(
    ISessionService sessionService,
    ILogger<LogoutUserService> logger
) : ILogoutUserService
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly ILogger<LogoutUserService> _logger = logger;

    public async Task<Result> ExecuteAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sessionId);

        _logger.LogInformation("Invalidating '{SessionId}' session…", sessionId.Trim());

        var session = await _sessionService.GetAsync(sessionId, cancellationToken);
        if (session is null)
        {
            _logger.LogInformation("Session already expired or invalidated!");

            return Result.Failure("Session already expired or invalidated!");
        }

        await _sessionService.InvalidateAsync(sessionId, cancellationToken);

         _logger.LogInformation( "User '{UserId}' successfully logged out.", session.UserId);

        return Result.Success();
    }
}
