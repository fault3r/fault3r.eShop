
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class EfUserRepository(
    EfDbContext efDbContext
) : IUserRepository
{
    private readonly EfDbContext _dbContext = efDbContext;

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await _dbContext.Users.FirstOrDefaultAsync(p => p.Email == email, ct);
    }

    public async Task CreateAsync(User user, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _dbContext.Users.AddAsync(user, ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Identity id, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        _dbContext.Users.Remove(new User(id));
        return Task.CompletedTask;
    }
}