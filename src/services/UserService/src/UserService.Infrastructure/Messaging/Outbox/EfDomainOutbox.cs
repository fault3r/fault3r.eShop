
using System;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging;
using UserService.Infrastructure.Exceptions.CrossCutting;
using UserService.Infrastructure.Exceptions.Messaging.Outbox;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfDomainOutbox : IDomainOutbox
{
    private readonly EfDbContext _dbContext;

    public EfDomainOutbox(EfDbContext efDbContext)
    {
        _dbContext = efDbContext
            ?? throw new MissingDbContextException();
    }

    public async Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        if (events is null)
            throw new MissingOutboxEventException();

        if (!events.Any()) return;

        if (events.Any(e => e is null))
            throw new MissingOutboxEventException();

        if (string.IsNullOrWhiteSpace(correlationId))
            throw new MissingCorrelationIdException();

        var messages = events
            .Select(e => OutboxMessage.FromEvent(e, correlationId))
            .ToList();

        await _dbContext.OutboxMessages
            .AddRangeAsync(messages, cancellationToken);
    }
}
