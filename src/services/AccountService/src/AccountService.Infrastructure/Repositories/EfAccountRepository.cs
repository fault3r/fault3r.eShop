
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Repositories;
using AccountService.Domain.ValueObjects;
using AccountService.Infrastructure.Exceptions.Persistence;
using AccountService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Repositories;

public class EfAccountRepository : IRepository
{
    private readonly AccountDbContext _db;

    public EfAccountRepository(AccountDbContext dbContext)
    {
        _db = dbContext
            ?? throw new DbContextException("DbContext is required");
    }
    public async Task CreateAsync(Account account, CancellationToken cancellationToken = default)
    {
        await _db.Accounts
            .AddAsync(account, cancellationToken);
    }

    public async Task<Account?> GetByIdAsync(Identity id, CancellationToken cancellationToken = default)
    {
        return await _db.Accounts
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Account?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _db.Accounts
            .FirstOrDefaultAsync(p => p.Email == email, cancellationToken);
    }

    public Task UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        _db.Accounts.Update(account);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Account account, CancellationToken cancellationToken = default)
    {
        _db.Accounts.Remove(account);
        return Task.CompletedTask;
    }
}