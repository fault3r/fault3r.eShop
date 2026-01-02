
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Identity id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default);
    Task CreateAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(Identity id, CancellationToken ct = default);
}
