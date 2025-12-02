
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User.Events;

public sealed record UserCreatedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email UserEmail { get; init; }

    public UserCreatedEvent(Identity userId, Email userEmail)
    {
        UserId = userId!;
        UserEmail = userEmail!;
    }

    public UserCreatedEvent(Guid eventId, DateTime occurredOn, Identity userId, Email userEmail)
        : base(eventId, occurredOn)
    {
        UserId = userId;
        UserEmail = userEmail;
    }
}
