
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Repositories;

public interface IRepository
{
    Task CreateAsync(Account account, CancellationToken cancellationToken = default);

    Task<Account?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default);
    Task<Account?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task UpdateAsync(Account account, CancellationToken cancellationToken = default);

    Task DeleteAsync(Account account, CancellationToken cancellationToken = default);
}
