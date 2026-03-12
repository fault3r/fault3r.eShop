
using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using UserService.Application.CrossCutting;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Outbox;
using UserService.Infrastructure.Persistence.Contexts;

namespace UserService.Infrastructure.Messaging.Outbox;

public sealed class EfPostgresEventOutbox(
    IDatabaseContext dbContext,
    IJsonSerializer jsonSerializer
) : IEventOutbox
{
    private readonly IDatabaseContext _dbContext = dbContext;
    private readonly IJsonSerializer _jsonSerializer = jsonSerializer;

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
            .Select(e => new OutboxMessage
            {
                Id = e.EventId,
                Type = e.GetType().Name,
                Payload = JsonSerializer.Serialize(e, e.GetType(), _jsonSerializer.DefaultOptions),
                Timestamp = e.OccurredOn,
                Processed = false,
                ProcessedAt = e.OccurredOn,
                CorrelationId = correlationId,
            });

        await _dbContext.OutboxMessages
            .AddRangeAsync(messages, cancellationToken);
    }

    public async Task<IEnumerable<OutboxMessage>> DequeueAsync(
        int count = 1,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.OutboxMessages
            .Where(p => !p.Processed)
            .OrderBy(p => p.Timestamp)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsProcessedAsync(
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.OutboxMessages
            .FirstOrDefaultAsync(p => p.Id == messageId, cancellationToken);

        if (message is not null && !message.Processed)
        {
            message.MarkAsProcessed();
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
