
using System;
using UserService.Domain.Outbox;
using UserService.Domain.Repositories;

namespace UserService.Domain.UnitOfWork;

public interface IUnitOfWork
{
    IOutbox Outbox { get; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    IUserRepository UserRepository { get; }
}
