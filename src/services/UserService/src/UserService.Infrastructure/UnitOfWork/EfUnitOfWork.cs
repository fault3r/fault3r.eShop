
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Messaging;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork(
    EfDbContext efDbContext,
    IDomainOutbox outbox,
    IDomainNotification notification,
    IUserRepository userRepository
) : IUnitOfWork
{
    private readonly EfDbContext _dbContext = efDbContext;
    public IDomainOutbox Outbox { get; } = outbox;
    public IDomainNotification Notification { get; } = notification;
    public IUserRepository UserRepository { get; } = userRepository;

    public async Task<int> CommitAsync(CancellationToken ct = default)
    {
        if (!_dbContext.ChangeTracker.HasChanges())
            return 0;

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(ct);

            try
            {
                var result = await _dbContext.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        });
    }
}
