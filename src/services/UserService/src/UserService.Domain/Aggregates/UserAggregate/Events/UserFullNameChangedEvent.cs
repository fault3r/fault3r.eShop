
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserFullNameChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public FullName NewFullName { get; }

    public UserFullNameChangedEvent(
        Identity userId,
        Email email,
        FullName newFullName)
    {
        ArgumentNullException.ThrowIfNull(userId);
        UserId = userId;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        ArgumentNullException.ThrowIfNull(newFullName);
        NewFullName = newFullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewFullName={NewFullName}";
}
