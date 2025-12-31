
using System;
using MediatR;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interfaces;

public interface IEventNotificationMapper
{
    INotification ToNotification(IDomainEvent domainEvent);
}
