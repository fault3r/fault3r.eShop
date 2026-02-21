
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task CreateAsync(User user, CancellationToken cancellationToken);

    Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);

    Task UpdateAsync(User user, CancellationToken cancellationToken);
    
    Task DeleteAsync(Identity id, CancellationToken cancellationToken);
}
