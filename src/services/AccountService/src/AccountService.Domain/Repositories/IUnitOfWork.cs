
using System;

namespace AccountService.Domain.Repositories;

public interface IUnitOfWork
{
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}
