
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Messaging.Outbox;
using UserService.Domain.Repositories;
using UserService.Domain.UnitOfWork;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Persistence.Contexts;

namespace UserService.Infrastructure.UnitOfWork;

public sealed class EfUnitOfWork(
    IDatabaseContext dbContext,
    IEventOutbox outbox,
    IUserRepository userRepository
) : IUnitOfWork
{
    private readonly IDatabaseContext _dbContext = dbContext;
    
    public IEventOutbox EventOutbox { get; } = outbox;
    public IUserRepository UserRepository { get; } = userRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(cancellationToken);
            
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            
            return result;
        });
    }
}
