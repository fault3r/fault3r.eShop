
using System;
using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Messaging.Notification;

namespace UserService.Application.Interfaces;

public interface INotificationMapper
{
    INotification FromEvent(IDomainEvent domainEvent, string correlationId);
    
    INotification FromNotificationMessage(NotificationMessage notificationMessage);
}
