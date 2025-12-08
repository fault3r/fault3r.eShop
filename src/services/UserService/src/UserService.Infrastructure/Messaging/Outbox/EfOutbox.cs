
using System;
using UserService.Domain.Interfaces;
using UserService.Domain.Outbox;
using UserService.Infrastructure.Exceptions.Logging;
using UserService.Infrastructure.Exceptions.Persistence;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfOutbox : IOutbox
{
    private readonly EfDbContext _db;
    private readonly Logging.ILogger _logger;

    public EfOutbox(
        EfDbContext efDbContext,  Logging.ILogger logger)
    {
        _db = efDbContext
            ?? throw new MissingDbContextException();

        _logger = logger
            ?? throw new MissingLoggerException();
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
                .Select(OutboxMessage.FromDomainEvent)
                .ToList();

            await _db.Set<OutboxMessage>().AddRangeAsync(messages, cancellationToken);
            
            _logger.Information($"enqueued {messages.Count} domain event(s) to outbox.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"failed to enqueue domain events!");
            throw;
        }
    }
}
