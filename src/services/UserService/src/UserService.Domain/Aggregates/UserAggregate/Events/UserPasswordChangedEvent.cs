
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserPasswordChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public FullName FullName { get; }

    public UserPasswordChangedEvent(
        Identity userId,
        Email email,
        FullName fullname)
    {
        ArgumentNullException.ThrowIfNull(userId);
        UserId = userId;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        ArgumentNullException.ThrowIfNull(fullname);
        FullName = fullname;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}";
}
