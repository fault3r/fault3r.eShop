using System;
using AccountService.Domain.Repositories;
using AccountService.Infrastructure.Exceptions.Persistence;
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

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (!_db.ChangeTracker.HasChanges())
            return true;

        int result = await SaveChangesTransactionalAsync(cancellationToken);

        return result > 0;
    }

    private async Task<int> SaveChangesTransactionalAsync(CancellationToken cancellationToken = default)
    {
        await using var process = await _db.Database
            .BeginTransactionAsync(cancellationToken);
        try
        {
            int changes = await _db.SaveChangesAsync(cancellationToken);
            
            await process.CommitAsync(cancellationToken);
            return changes;
        }
        catch 
        {
            await process.RollbackAsync(cancellationToken);
            return 0;
        }
    }
}
