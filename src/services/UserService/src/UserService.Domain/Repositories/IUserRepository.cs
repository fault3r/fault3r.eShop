
using System;
using UserService.Domain.Aggregates;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task CreateAsync(User User, CancellationToken cancellationToken = default);

    Task UpdateAsync(User User, CancellationToken cancellationToken = default);

    Task DeleteAsync(Identity id, CancellationToken cancellationToken = default);
}
