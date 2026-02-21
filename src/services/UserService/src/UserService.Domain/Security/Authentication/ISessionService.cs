
using System;

namespace UserService.Domain.Security.Authentication;

public interface ISessionService
{
    Task CreateAsync(SessionData session, CancellationToken cancellationToken);

    Task<SessionData?> GetAsync(string sessionId, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(string sessionId, CancellationToken cancellationToken);

    Task UpdateAsync(SessionData session, CancellationToken cancellationToken);
    
    Task InvalidateAsync(string sessionId, CancellationToken cancellationToken);
    Task InvalidateAllAsync(string userId, CancellationToken cancellationToken);
}
