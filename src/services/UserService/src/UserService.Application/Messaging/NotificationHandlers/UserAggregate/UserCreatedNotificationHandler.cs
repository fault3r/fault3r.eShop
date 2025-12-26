
using System;
using MediatR;
using UserService.Application.Messaging.Notifications.UserAggregate;

namespace UserService.Application.Messaging.NotificationHandlers.UserAggregate;

public sealed class UserCreatedNotificationHandler
    : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        //send welcome email
        throw new NotImplementedException();
    }
}
