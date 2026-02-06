
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Persistence;

public sealed class EfPostgresDbContext(
    DbContextOptions<EfPostgresDbContext> dbcOptions
) : DbContext(dbcOptions)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(EfPostgresDbContext).Assembly);
    }
}
