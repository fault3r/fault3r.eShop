
using System;
using UserService.Domain.Messaging.Outbox;
using UserService.Domain.Repositories;

namespace UserService.Domain.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken cancellationToken);
    
    IEventOutbox EventOutbox { get; }

    IUserRepository UserRepository { get; }
}
