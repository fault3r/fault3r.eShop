
using System;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Infrastructure.Messaging.Outbox;

public static class OutboxTypeResolver
{
    private static readonly Dictionary<string, Type> _types = new()
    {
        ["UserRegisteredEvent"] = typeof(UserRegisteredEvent),
        ["UserFullNameChangedEvent"] = typeof(UserFullNameChangedEvent),
        ["UserPasswordChangedEvent"] = typeof(UserPasswordChangedEvent)
    };

    public static Type? Resolve(string typeName)
        => _types.TryGetValue(typeName, out var type) ? type : null;
}
