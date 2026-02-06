
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Outbox;
using UserService.Infrastructure.Persistence;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfPostgresEventOutbox(
    EfPostgresDbContext efDbContext
) : IEventOutbox
{
    private readonly EfPostgresDbContext _dbContext = efDbContext;

    public async Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentException.ThrowIfNullOrWhiteSpace(correlationId);

        if (!events.Any()) return;

        if (events.Any(e => e is null))
            throw new ArgumentException($"{nameof(events)} contains null element");

        var messages = events
            .Select(e => OutboxMessage.FromEvent(e, correlationId));

        await _dbContext.OutboxMessages
            .AddRangeAsync(messages, cancellationToken);
    }

    public async Task<IEnumerable<OutboxMessage>> DequeueAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.OutboxMessages
            .Where(p => !p.Processed)
            .OrderBy(p => p.Timestamp)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.OutboxMessages.FirstOrDefaultAsync(p => p.Id == messageId, cancellationToken);

        if (message is not null && !message.Processed)
        {
            message.MarkAsProcessed();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
