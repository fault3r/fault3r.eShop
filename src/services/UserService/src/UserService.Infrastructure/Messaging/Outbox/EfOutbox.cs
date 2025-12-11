
using System;
using Microsoft.Extensions.Logging;
using UserService.Domain.Interfaces;
using UserService.Domain.Outbox;
using UserService.Infrastructure.Correlation;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfOutbox : IOutbox
{
    private readonly EfDbContext _db;

    private readonly ICorrelationContext _correlation;

    public EfOutbox(
        EfDbContext efDbContext,
        ICorrelationContext correlation)
    {
        _db = efDbContext
            ?? throw new MissingDbContextException();

        _correlation = correlation;
    }


    public async Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        if (domainEvents is null)
            throw new MissingEventsException();

        if (!domainEvents.Any()) return;

        if (domainEvents.Any(e => e is null))
            throw new MissingEventException();

        var messages = domainEvents
            .Select(e => OutboxMessage.FromDomainEvent(e, _correlation.CorrelationId))
            .ToList();

        await _db.Set<OutboxMessage>().AddRangeAsync(messages, cancellationToken);
    }
}
