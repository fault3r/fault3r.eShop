
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Infrastructure.Messaging.Outbox;
using AccountService.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence;

public class AccountDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountConfiguration());
        base.OnModelCreating(builder);
    }
}

