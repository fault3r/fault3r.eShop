
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class EfUserRepository : IUserRepository
{
    private readonly EfDbContext _dbContext;

    public EfUserRepository(
        EfDbContext efDbContext)
    {
        _dbContext = efDbContext
            ?? throw new MissingDbContextException();
    }

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        if (id is null)
            throw new RepositoryArgumentException();

        return await _dbContext.Users.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        if (email is null)
            throw new RepositoryArgumentException();

        return await _dbContext.Users.FirstOrDefaultAsync(p => p.Email == email);
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users
            .AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Users
            .Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Identity id, CancellationToken cancellationToken = default)
    {
        _dbContext.Users
            .Remove(new User(id));
        return Task.CompletedTask;
    }
}