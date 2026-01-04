
using System;
using UserService.Application.Security.Authentication;

namespace UserService.Application.Interfaces;

public interface ISessionService
{
    Task CreateAsync(SessionData session, CancellationToken ct = default);

    Task<SessionData?> GetAsync(string sessionId, CancellationToken ct = default);
    Task<bool> ExistAsync(string sessionId, CancellationToken ct = default);

    Task UpdateAsync(SessionData session, CancellationToken ct = default);
    
    Task InvalidateAsync(string sessionId, CancellationToken ct = default);
    Task InvalidateAllAsync(string userId, CancellationToken ct = default);
}
