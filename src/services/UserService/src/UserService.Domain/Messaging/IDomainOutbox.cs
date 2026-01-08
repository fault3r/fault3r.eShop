
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface IDomainOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> events,
        string correlationId,
        CancellationToken cancellationToken = default
    );    
}
