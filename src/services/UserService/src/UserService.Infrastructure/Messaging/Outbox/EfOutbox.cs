
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

    private readonly ILogger<EfDbContext> _logger;

    public EfOutbox(
        EfDbContext efDbContext,
        ILogger<EfDbContext> logger,
        ICorrelationContext correlation)
    {
        _db = efDbContext
            ?? throw new MissingDbContextException();

        _logger = logger;
        _correlation = correlation;
    }


    public async Task EnqueueAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        try
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

            _logger.LogInformation("enqueued {Count} domain event(s) to outbox.", messages.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"failed to enqueue domain events!");
            throw;
        }
    }
}
