
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserActivatedEvent : DomainEvent
{
    public Identity UserId { get; init; }

    public UserActivatedEvent(Identity userId)
        : base()
    {
        UserId = userId;
    }

    public UserActivatedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId)
        : base(eventId, occurredOn)
    {
        UserId = userId;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Activated=true";
}
