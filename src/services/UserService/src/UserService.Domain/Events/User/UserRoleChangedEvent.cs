
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserRoleChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Role NewRole { get; init; }

    public UserRoleChangedEvent(Identity userId, Role newRole)
        : base()
    {
        UserId = userId;
        NewRole = newRole;
    }

    public UserRoleChangedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId,
        Role newRole)
        : base(eventId, occurredOn)
    {
        UserId = userId;
        NewRole = newRole;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, NewRole={NewRole}";
}
