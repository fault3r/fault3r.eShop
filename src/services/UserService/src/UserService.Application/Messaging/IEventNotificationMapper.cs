
using System;
using MediatR;
using UserService.Domain.Interfaces;

namespace UserService.Application.Messaging;

public interface IEventNotificationMapper
{
    INotification ToNotification(IDomainEvent domainEvent);
}
