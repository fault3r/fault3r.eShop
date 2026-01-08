
using System;
using UserService.Application.Security.Authentication;

namespace UserService.Application.Interfaces;

public interface ISessionService
{
    Task CreateAsync(SessionData session, CancellationToken cancellationToken = default);

    Task<SessionData?> GetAsync(string sessionId, CancellationToken cancellationToken = default);
    Task<bool> ExistAsync(string sessionId, CancellationToken cancellationToken = default);

    Task UpdateAsync(SessionData session, CancellationToken cancellationToken = default);
    
    Task InvalidateAsync(string sessionId, CancellationToken cancellationToken = default);
    Task InvalidateAllAsync(string userId, CancellationToken cancellationToken = default);
}
