
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserRegisteredEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public FullName FullName { get; }

    public UserRegisteredEvent(
        Identity userId,
        Email email,
        FullName fullName)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(fullName);
        
        UserId = userId;
        Email = email;
        FullName = fullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}";
}