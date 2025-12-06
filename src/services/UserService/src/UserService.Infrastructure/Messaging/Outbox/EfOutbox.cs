
using System;
using UserService.Domain.Interfaces;
using UserService.Domain.Outbox;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfOutbox : IOutbox
{
    private readonly EfDbContext _db;

    public EfOutbox(EfDbContext efDbContext)
    {
        _db = efDbContext
            ?? throw new MissingDbContextException();
    }

    public async Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        if (domainEvents is null)
            throw new MissingEventsException();

        if(!domainEvents.Any()) return;

        if(domainEvents.Any(e => e is null))
            throw new MissingEventException();

        var messages = domainEvents
            .Select(OutboxMessage.FromDomainEvent)
            .ToList();

        await _db.Set<OutboxMessage>().AddRangeAsync(messages, cancellationToken);
    }
}
