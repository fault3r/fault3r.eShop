
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging.Outbox;

public interface IEventOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken
    );

    Task<IEnumerable<EventMessage>> DequeueAsync(
        int count,
        CancellationToken cancellationToken
    );

    Task MarkAsProcessedAsync(
        Guid messageId,
        CancellationToken cancellationToken
    );
}
