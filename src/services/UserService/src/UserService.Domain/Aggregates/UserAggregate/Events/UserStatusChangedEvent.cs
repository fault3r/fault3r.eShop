
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserStatusChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public Status NewStatus { get; }

    public UserStatusChangedEvent(
        Identity userId,
        Email email,
        Status status)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(status);

        UserId = userId;
        Email = email;
        NewStatus = status;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewStatus={NewStatus}";
}
