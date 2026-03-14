
using System;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Messaging.Outbox;

namespace UserService.Infrastructure.Persistence.Contexts;

public interface IDatabaseContext
{
     DbSet<User> Users { get; }
     DbSet<EventMessage> Events { get; }

     ChangeTracker ChangeTracker { get; }
     DatabaseFacade Database { get; }

     DbSet<OutboxMessage> OutboxMessages { get; }
     DbSet<InboxState> InboxStates { get; }
     DbSet<OutboxState> OutboxStates { get; }

     Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
