
using System;
using UserService.Application.Messaging.Notification;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Interfaces;

public interface INotificationFactory
{
    Notification FromEvent(IDomainEvent domainEvent, string correlationId);
    
    Notification FromNotificationMessage(NotificationMessage notificationMessage);
}
