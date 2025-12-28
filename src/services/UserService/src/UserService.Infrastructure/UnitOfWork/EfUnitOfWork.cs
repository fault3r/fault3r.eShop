
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Messaging;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly EfDbContext _dbContext;
    public IDomainOutbox Outbox { get; }
    public IDomainNotification Notification { get; }
    public IUserRepository UserRepository { get; }

    public EfUnitOfWork(
        EfDbContext efDbContext,
        IDomainOutbox outbox,
        IDomainNotification notification,
        IUserRepository userRepository)
    {
        ArgumentNullException.ThrowIfNull(efDbContext);
        ArgumentNullException.ThrowIfNull(outbox);
        ArgumentNullException.ThrowIfNull(notification);
        ArgumentNullException.ThrowIfNull(userRepository);

        _dbContext = efDbContext;
        Outbox = outbox;
        Notification = notification;
        UserRepository = userRepository;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (!_dbContext.ChangeTracker.HasChanges())
            return 0;

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}
