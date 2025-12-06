
using System;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Aggregates;
using UserService.Infrastructure.Messaging.Outbox;

namespace UserService.Infrastructure.Persistence;

public class PostgresDbContext(
    DbContextOptions<PostgresDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            typeof(PostgresDbContext).Assembly);
    }
}
