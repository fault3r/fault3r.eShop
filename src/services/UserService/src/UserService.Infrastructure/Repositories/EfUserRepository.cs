
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
    private readonly EfDbContext _dbContext;
    private readonly ILogger<EfUserRepository> _logger;

    public EfUserRepository(
        EfDbContext efDbContext,
        ILogger<EfUserRepository> logger)
    {
        _dbContext = efDbContext
            ?? throw new MissingDbContextException();

        _logger = logger;
    }

    private async Task<User?> QueryAsync(
        Expression<Func<User, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("executing query");

        try
        {
            var result = await _dbContext.Users.FirstOrDefaultAsync(expression, cancellationToken);

            _logger.LogInformation("query executed successfully");

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "query execution failed");

            throw new PersistenceException();
        }
    }

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        return await QueryAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await QueryAsync(p => p.Email == email, cancellationToken);
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<User>()
            .AddAsync(user, cancellationToken);
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<User>()
            .Update(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Identity id, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<User>()
            .Remove(new User(id));            
        return Task.CompletedTask;
    }
}