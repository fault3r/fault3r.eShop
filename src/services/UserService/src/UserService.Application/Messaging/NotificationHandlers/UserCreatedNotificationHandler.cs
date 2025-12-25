
using System;
using MediatR;
using UserService.Application.Messaging.Notifications;

namespace UserService.Application.Messaging.NotificationHandlers;

public sealed class UserCreatedNotificationHandler
    : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        //send welcome email
        throw new NotImplementedException();
    }
}
