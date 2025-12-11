
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class EfUserRepository(EfDbContext efDbContext) : IUserRepository
{
    private readonly EfDbContext _db = efDbContext
        ?? throw new MissingDbContextException();
        
    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _db.Users.AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _db.Users.Update(user);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Identity id, CancellationToken cancellationToken = default)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (user is not null)
            _db.Users.Remove(user);
    }

    public Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        return _db.Users
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return _db.Users
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }
}
