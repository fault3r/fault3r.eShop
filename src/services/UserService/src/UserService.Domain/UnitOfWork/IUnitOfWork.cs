
using System;
using UserService.Domain.Repositories;

namespace UserService.Domain.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository Users { get; init;}

    Task<int> CommitChangesAsync(CancellationToken cancellationToken = default);
}
