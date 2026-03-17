
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserRoleChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public Role NewRole { get; }

    public UserRoleChangedEvent(
        Identity userId,
        Email email,
        Role role)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(role);

        UserId = userId;
        Email = email;
        NewRole = role;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewRole={NewRole}";
}
