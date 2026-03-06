
using System;
using MassTransit;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Messaging.Bus;

public class MassTransitMessageBus(
    IPublishEndpoint publishEndpoint
) : IMessageBus
{
    private readonly IPublishEndpoint _publisher = publishEndpoint;

    public async Task PublishAsync(
        IDomainEvent @event,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _publisher.Publish(@event, cancellationToken);
    }
}
