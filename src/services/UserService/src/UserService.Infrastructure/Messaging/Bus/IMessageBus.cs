
using System;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Messaging.Bus;

public interface IMessageBus
{
    Task PublishAsync(
        IDomainEvent @event,
        CancellationToken cancellationToken
    );
}