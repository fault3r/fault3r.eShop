
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Messaging;

public interface IDomainOutbox
{
    Task EnqueueAsync(
        IEnumerable<IDomainEvent> domainEvents,
        string correlationId,
        CancellationToken cancellationToken = default
    );    
}
