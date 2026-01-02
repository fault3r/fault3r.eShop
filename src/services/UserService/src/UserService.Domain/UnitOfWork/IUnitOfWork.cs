
using System;
using UserService.Domain.Messaging;
using UserService.Domain.Repositories;

namespace UserService.Domain.UnitOfWork;

public interface IUnitOfWork
{
    Task<int> CommitAsync(CancellationToken ct = default);
    
    IDomainOutbox Outbox { get; }
    IDomainNotification Notification { get; }

    IUserRepository UserRepository { get; }
}
