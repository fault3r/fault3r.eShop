
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserPasswordChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }

    public UserPasswordChangedEvent(Identity userId)
        : base()
    {
        UserId = userId;
    }

    public UserPasswordChangedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId)
        : base(eventId, occurredOn)
    {
        UserId = userId;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, PasswordChanged=true";
}
