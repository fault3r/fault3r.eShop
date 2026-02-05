
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Outbox;

public interface IEventOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default
    );

    Task<IEnumerable<OutboxMessage>> DequeueAsync(
        CancellationToken cancellationToken = default
    );

    Task MarkAsProcessedAsync(
        Guid messageId,
        CancellationToken cancellationToken = default
    );
}
