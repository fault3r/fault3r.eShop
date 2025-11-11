
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Aggregates.Account;
using AccountService.Infrastructure.Messaging.Outbox;
using AccountService.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence;

public class AccountDbContext : DbContext
{
    public DbSet<Account> Accounts
        => Set<Account>();

    public DbSet<OutboxMessage> OutboxMessages
        => Set<OutboxMessage>();

    public AccountDbContext(DbContextOptions<AccountDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new AccountConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Account>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var outboxMessages = domainEvents
            .Select(OutboxMessage.FromDomainEvent)
            .ToList();

        Set<OutboxMessage>().AddRange(outboxMessages);

        foreach (var entry in ChangeTracker.Entries<AggregateRoot>())
            entry.Entity.ClearEvents();
                        
        return await base.SaveChangesAsync(cancellationToken);
    }
}

