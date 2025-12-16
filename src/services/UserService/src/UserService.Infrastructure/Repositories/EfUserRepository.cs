
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
    private readonly IUnitOfWork _uow;
    private readonly ILogger<EfUserRepository> _logger;

    public EfUserRepository(
        EfDbContext efDbContext,
        IUnitOfWork uow,
        ILogger<EfUserRepository> logger)
    {
        _db = efDbContext;
        _uow = uow;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        return await _uow.QueryAsync(async ct =>
        {
            return await _db.Users.FirstOrDefaultAsync(p => p.Id == id, ct);
        }, cancellationToken);

    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _uow.QueryAsync(async ct =>
        {
            return await _db.Users.FirstOrDefaultAsync(p => p.Email == email, ct);
        }, cancellationToken);
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
