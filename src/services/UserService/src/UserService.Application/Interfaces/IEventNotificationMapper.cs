
using System;
using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Interfaces;

public interface IEventNotificationMapper
{
    INotification FromEvent(IDomainEvent domainEvent);
    INotification FromNotificationMessage(NotificationMessage notificationMessage);
}
