
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
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
        _db = efDbContext
            ?? throw new MissingDbContextException();

        _logger = logger;
    }

    private async Task<User?> FindByExpression(
        Expression<Func<User, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Executing query");

        try
        {
            var result = await _db.Users.FirstOrDefaultAsync(expression, cancellationToken);

            _logger.LogInformation("Query executed");

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Database query failed.");

            throw new PersistenceException();
        }
    }

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        return await FindByExpression(p => p.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await FindByExpression(p => p.Email == email, cancellationToken);
    }

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
        var user = await GetByIdAsync(id, cancellationToken);
        if (user is not null)
            _db.Users.Remove(user);
    }
}
