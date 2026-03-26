
using System;
using UserService.Domain.Aggregates.UserAggregate.Events;

namespace UserService.Infrastructure.Messaging.Outbox;

public static class EventTypeResolver
{
    private static readonly Dictionary<string, Type> types = new()
    {
        [nameof(UserRegisteredEvent)] = typeof(UserRegisteredEvent),
        [nameof(UserFullNameChangedEvent)] = typeof(UserFullNameChangedEvent),
        [nameof(UserPasswordChangedEvent)] = typeof(UserPasswordChangedEvent),
    };

    public static Type? Resolve(string typeName)
        => types.TryGetValue(typeName, out var type) ? type : null;
}
