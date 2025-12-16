
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Repositories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Repositories;

public sealed class EfUserRepository : IUserRepository
{
    private readonly EfDbContext _db;
    private readonly ILogger<EfUserRepository> _logger;

    public EfUserRepository(
        EfDbContext efDbContext,
        ILogger<EfUserRepository> logger)
    {
        _db = efDbContext;
        _logger = logger;
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _db.Users.AddAsync(user, cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _db.Users.Update(user);
    }

    public async Task DeleteAsync(Identity id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user is not null)
            _db.Users.Remove(user);
    }

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        return await QueryAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await QueryAsync(p => p.Email == email, cancellationToken);
    }

    private async Task<User?> QueryAsync(
        Expression<Func<User, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _db.Users
                .FirstOrDefaultAsync(expression, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to execute query");

            throw new PersistenceException();
        }
    }
}
