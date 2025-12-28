
using System;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfDomainOutbox : IDomainOutbox
{
    private readonly EfDbContext _dbContext;

    public EfDomainOutbox(EfDbContext efDbContext)
    {
        ArgumentNullException.ThrowIfNull(efDbContext);

        _dbContext = efDbContext;
    }

    public async Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);

        if (!events.Any()) return;

        if (events.Any(e => e is null))
            throw new ArgumentException($"{nameof(events)} contains null element");

        ArgumentNullException.ThrowIfNull(correlationId);

        var messages = events
            .Select(e => OutboxMessage.FromEvent(e, correlationId))
            .ToList();

        await _dbContext.OutboxMessages
            .AddRangeAsync(messages, cancellationToken);
    }
}
