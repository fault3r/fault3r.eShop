
using System;
using MediatR;
using UserService.Domain.Interfaces;

namespace UserService.Application.Interfaces;

public interface IDomainEventNotificationMapper
{
    INotification Map(IDomainEvent domainEvent);
}
