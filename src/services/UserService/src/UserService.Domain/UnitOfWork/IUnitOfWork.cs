
using System;
using UserService.Domain.Outbox;
using UserService.Domain.Repositories;

namespace UserService.Domain.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository Users { get; init;}
        
    IOutbox Outbox { get; init; }

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}
