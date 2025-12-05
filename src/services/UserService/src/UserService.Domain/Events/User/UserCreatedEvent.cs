
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserCreatedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email Email { get; init; }

    public UserCreatedEvent(Identity userId, Email email)
        : base()
    {
        UserId = userId!;
        Email = email!;
    }

    public UserCreatedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId,
        Email email)
        : base(eventId, occurredOn)
    {
        UserId = userId;
        Email = email;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, UserEmail={Email}";
}