
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserEmailChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email NewEmail { get; }
    public FullName FullName { get; }

    public UserEmailChangedEvent(
        Identity userId,
        Email newEmail,
        FullName fullName)
    {
        ArgumentNullException.ThrowIfNull(userId);
        UserId = userId;
        
        ArgumentNullException.ThrowIfNull(newEmail);
        NewEmail = newEmail;

        ArgumentNullException.ThrowIfNull(fullName);
        FullName = fullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, NewEmail={NewEmail}, FullName={FullName}";
}
