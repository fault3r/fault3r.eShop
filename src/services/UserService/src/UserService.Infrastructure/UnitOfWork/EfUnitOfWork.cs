using Microsoft.EntityFrameworkCore;
using UserService.Domain.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly EfDbContext _dbContext;

    public IUserRepository Users { get; init; }
    public IOutbox Outbox { get; init; }

    public EfUnitOfWork(
        EfDbContext efDbContext, IUserRepository users, IOutbox outbox)
    {
        _dbContext = efDbContext;
        Users = users;
        Outbox = outbox;
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (!_dbContext.ChangeTracker.HasChanges())
            return 0;

        var executionStrategy = _dbContext.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(async () =>
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
