
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.Interfaces;
using AccountService.Domain.Services;
using AccountService.Infrastructure.Exceptions.Persistence;
using AccountService.Infrastructure.Messaging.Outbox;
using AccountService.Infrastructure.Persistence;

namespace AccountService.Infrastructure.Repositories;

public class EfOutbox : IOutbox
{
    private readonly AccountDbContext _db;

    public EfOutbox(AccountDbContext dbContext)
    {
        _db = dbContext
            ?? throw new DbContextException("DbContext is required");
    }

    public async Task DispatchAsync(ReadOnlyCollection<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        if (!domainEvents.Any())
            return;

        var outboxMessages = domainEvents
            .Select(OutboxMessage.FromDomainEvent)
            .ToList();

        await _db.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
    }
}