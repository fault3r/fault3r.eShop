
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserStatusChangedEvent : DomainEvent
{
    public Identity UserId { get;  }
    public Email Email { get;  }
    public FullName FullName { get; }
    public Status NewStatus { get; }

    public UserStatusChangedEvent(
        Identity userId,
        Email email,
        FullName fullName,
        Status status)
    {
        ArgumentNullException.ThrowIfNull(userId);
        UserId = userId;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        ArgumentNullException.ThrowIfNull(fullName);
        FullName = fullName;

        ArgumentNullException.ThrowIfNull(status);
        NewStatus = status;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}, NewStatus={NewStatus}";
}
