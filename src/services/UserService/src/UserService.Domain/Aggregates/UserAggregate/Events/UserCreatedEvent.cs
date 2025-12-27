
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserCreatedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public FullName FullName { get; }

    public UserCreatedEvent(
        Identity userId,
        Email email,
        FullName fullName)
    {
        ArgumentNullException.ThrowIfNull(userId);
        UserId = userId;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;
        
        ArgumentNullException.ThrowIfNull(fullName);
        FullName = fullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}";
}