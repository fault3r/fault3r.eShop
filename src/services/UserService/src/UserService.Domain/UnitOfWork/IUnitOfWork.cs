
using System;
using UserService.Domain.Messaging;
using UserService.Domain.Repositories;

namespace UserService.Domain.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
    
    IEventOutbox EventOutbox { get; }
    INotificationOutbox NotificationOutbox { get; }

    IUserRepository UserRepository { get; }
}
