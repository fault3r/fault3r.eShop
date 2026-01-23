
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface IEventOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default
    );    
}
