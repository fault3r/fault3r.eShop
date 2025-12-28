
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserRoleChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public FullName FullName { get; }
    public Role NewRole { get; }

    public UserRoleChangedEvent(
        Identity userId,
        Email email,
        FullName fullName,
        Role newRole)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(fullName);
        ArgumentNullException.ThrowIfNull(newRole);

        UserId = userId;
        Email = email;
        FullName = fullName;
        NewRole = newRole;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}, NewRole={NewRole}";
}
