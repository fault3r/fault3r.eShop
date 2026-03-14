
using System;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Persistence.Contexts;

public sealed class EfPostgresDbContext(
    DbContextOptions<EfPostgresDbContext> dbcOptions
) : DbContext(dbcOptions), IDatabaseContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<EventMessage> Events => Set<EventMessage>();

    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<InboxState> InboxStates { get; set; }
    public DbSet<OutboxState> OutboxStates { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(EfPostgresDbContext).Assembly);

        builder.Entity<OutboxMessage>().HasKey(p => p.MessageId);
        builder.AddInboxStateEntity();
        builder.AddOutboxStateEntity();
    }
}
