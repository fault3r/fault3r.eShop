
using System;
using UserService.Application.Security.Authentication;

namespace UserService.Application.Interfaces;

public interface ISessionService
{
    Task CreateSessionAsync(
        SessionData session,
        CancellationToken cancellationToken = default
    );

    Task<SessionData?> GetSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default
    );

    Task<bool> SessionExistAsync(
        string sessionId,
        CancellationToken cancellationToken = default
    );

    Task UpdateSessionAsync(
        SessionData session,
        CancellationToken cancellationToken = default
    );

    Task InvalidateSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default
    );

    Task InvalidateAllUserSessionsAsync(
        string userId,
        CancellationToken cancellationToken = default
    );
}
