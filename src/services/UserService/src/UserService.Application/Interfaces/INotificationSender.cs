
using System;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interfaces;

public interface INotificationSender
{
    Task PublishAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
