
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserLockedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email Email { get; init; }

    public UserLockedEvent(Identity userId, Email email)
        : base()
    {
        UserId = userId
            ?? throw new MissingIdentityException();
        Email = email
            ?? throw new MissingEmailException();
    }

    public UserLockedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId,
        Email email)
        : base(eventId, occurredOn)
    {
        UserId = userId
            ?? throw new MissingIdentityException();
        Email = email
            ?? throw new MissingEmailException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Locked=true";
}
