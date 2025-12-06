
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserEmailChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email NewEmail { get; init; }

    public UserEmailChangedEvent(Identity userId, Email newEmail)
        : base()
    {
        UserId = userId
            ?? throw new MissingIdentityException();
        NewEmail = newEmail
            ?? throw new MissingEmailException();
    }

    public UserEmailChangedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId,
        Email newEmail)
        : base(eventId, occurredOn)
    {
        UserId = userId
            ?? throw new MissingIdentityException();
        NewEmail = newEmail
            ?? throw new MissingEmailException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, NewEmail={NewEmail}";
}
