
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Persistence.Contexts;

public interface IDatabaseContext
{
     DbSet<User> Users { get; }
     DbSet<OutboxMessage> OutboxMessages { get; }

     ChangeTracker ChangeTracker { get; }
     DatabaseFacade Database { get; }
     EntityEntry Entry(object entity);

     Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
