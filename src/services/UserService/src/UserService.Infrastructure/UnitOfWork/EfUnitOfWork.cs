
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Domain.Exceptions.Abstraction.AggregateRoot;
using UserService.Domain.Messaging;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Exceptions.Messaging.Notification;
using UserService.Infrastructure.Exceptions.Messaging.Outbox;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly EfDbContext _dbContext;
    public IDomainOutbox Outbox { get; } 
    public IUserRepository UserRepository { get; }
    public IDomainNotification Notification { get; }
    private readonly ILogger<EfUnitOfWork> _logger;

    public EfUnitOfWork(
        EfDbContext efDbContext,
        IDomainOutbox outbox,
        IUserRepository userRepository,
        IDomainNotification notification,
        ILogger<EfUnitOfWork> logger)
    {
        _dbContext = efDbContext
            ?? throw new MissingDbContextException();

        Outbox = outbox
            ?? throw new MissingDomainOutboxException();

        UserRepository = userRepository
            ?? throw new MissingUserRepositoryException();

        Notification = notification
            ?? throw new MissingDomainNotificationException();

        _logger = logger;
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
                _logger.LogInformation("Committing changes..");

                var result = await _dbContext.SaveChangesAsync(cancellationToken);
                
                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Successfully committed {Count} change(s).", result);

                return result;
            }
            catch (Exception exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                _logger.LogError(exception, "Failed to commit changes!");

                throw new PersistenceException();
            }
        });
    }    
}
