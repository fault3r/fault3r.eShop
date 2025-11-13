using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Repositories;
using AccountService.Infrastructure.Exceptions.Persistence;
using AccountService.Infrastructure.Messaging.Outbox;
using AccountService.Infrastructure.Persistence;

namespace AccountService.Infrastructure.Repositories;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly AccountDbContext _db;

    public EfUnitOfWork(AccountDbContext dbContext)
    {
        _db = dbContext
            ?? throw new DbContextException("DbContext is required");
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = _db.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        if (!domainEvents.Any())
            return await _db.SaveChangesAsync(cancellationToken);

        var outboxMessages = domainEvents
            .Select(OutboxMessage.FromDomainEvent)
            .ToList();

        int result = 0;
        await ExecuteTransactionalAsync(async () =>
        {
            await _db.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
            result = await _db.SaveChangesAsync(cancellationToken);
        }, cancellationToken);

        if (result <= 0)
            throw new UnitOfWorkException("failed to persist changes");

        foreach (var entry in _db.ChangeTracker.Entries<AggregateRoot>())
            entry.Entity.ClearEvents();

        return result;
    }

    private async Task ExecuteTransactionalAsync(Func<Task> transaction, CancellationToken cancellationToken = default)
    {
        await using var process
            = await _db.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await transaction();
            await process.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await process.RollbackAsync(cancellationToken);
            throw new UnitOfWorkException($"transaction failed: {e.Message}");
        }
    }
}
