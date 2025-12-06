
using System;
using System.Reflection;
using UserService.Domain.Events.User;
using UserService.Domain.Factories;
using UserService.Domain.ValueObjects;
using UserService.Infrastructure.Messaging.Outbox;
using UserService.Infrastructure.Persistence;

namespace UserService.Tests;

public class MainTests
{
    [Fact]
    public void TestName()
    {

        var ass = Assembly.GetExecutingAssembly();
        var assName = ass.FullName;
        var dbAss = typeof(PostgresDbContext).Assembly.FullName;
        var dmAss = typeof(UserCreatedEvent).Assembly.FullName;
        var otAss = typeof(OutboxMessage).Assembly.FullName;
    }
}
