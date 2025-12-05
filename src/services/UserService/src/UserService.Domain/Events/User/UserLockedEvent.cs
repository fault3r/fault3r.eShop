
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserLockedEvent : DomainEvent
{
    public Identity UserId { get; init; }

    public UserLockedEvent(Identity userId)
        : base()
    {
        UserId = userId;
    }

    public UserLockedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId)
        : base(eventId, occurredOn)
    {
        UserId = userId;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Locked=true";
}
