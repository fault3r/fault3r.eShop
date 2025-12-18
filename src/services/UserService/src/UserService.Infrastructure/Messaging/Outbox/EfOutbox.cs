
using System;
using UserService.Domain.Interfaces;
using UserService.Domain.Outbox;
using UserService.Infrastructure.Exceptions.CrossCutting;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfOutbox : IOutbox
{
    private readonly EfDbContext _dbContext;

    public EfOutbox(EfDbContext efDbContext)
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
