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
        if (!_db.ChangeTracker.HasChanges())
            return await _db.SaveChangesAsync();

        int result = 0;
        await ExecuteTransactionalAsync(async () =>
        {
            result = await _db.SaveChangesAsync(cancellationToken);

            foreach (var entry in _db.ChangeTracker.Entries<AggregateRoot>())
                entry.Entity.ClearEvents();

        }, cancellationToken);

        if (result <= 0)
            throw new UnitOfWorkException("Failed to persist changes");

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
