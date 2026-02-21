
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class EfUserRepository(
    EfPostgresDbContext efDbContext
) : IUserRepository
{
    private readonly EfPostgresDbContext _dbContext = efDbContext;

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        await _dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(email);

        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        _dbContext.Users.Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Identity id, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(id);

        _dbContext.Users.Remove(new User(id));
        return Task.CompletedTask;
    }
}