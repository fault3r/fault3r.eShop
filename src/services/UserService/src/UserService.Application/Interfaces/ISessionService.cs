
using System;
using UserService.Application.Security.Authentication;

namespace UserService.Application.Interfaces;

public interface ISessionService
{
    Task CreateSessionAsync(SessionData session, CancellationToken ct = default);
    Task UpdateSessionAsync(SessionData session, CancellationToken ct = default);

    Task<SessionData?> GetSessionAsync(string sessionId, CancellationToken ct = default);
    Task<bool> SessionExistAsync(string sessionId, CancellationToken ct = default);

    Task InvalidateSessionAsync(string sessionId, CancellationToken ct = default);
    Task InvalidateAllUserSessionsAsync(string userId, CancellationToken ct = default);
}
