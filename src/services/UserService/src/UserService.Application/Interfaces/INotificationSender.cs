
using System;
using UserService.Application.Messaging.Notification;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interfaces;

public interface INotificationSender
{
    Task SendAsync(
        IDomainEvent @event,
        string correlationId,
        CancellationToken cancellationToken = default
    );
}
