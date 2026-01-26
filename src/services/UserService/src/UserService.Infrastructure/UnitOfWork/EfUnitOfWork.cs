
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Messaging.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork(
    EfDbContext efDbContext,
    IEventOutbox outbox,
    IUserRepository userRepository
) : IUnitOfWork
{
    private readonly EfDbContext _dbContext = efDbContext;
    public IEventOutbox EventOutbox { get; } = outbox;
    public IUserRepository UserRepository { get; } = userRepository;

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
