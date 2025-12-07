
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Events.User;

public sealed record UserPasswordChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email Email { get; init; }    

    public UserPasswordChangedEvent(
        Identity userId,
        Email email,
        Guid? eventId = null,
        DateTime? occurredOn = null)
        : base(eventId, occurredOn)
    {
        UserId = userId
            ?? throw new MissingIdentityException();
        Email = email
            ?? throw new MissingEmailException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}";
}
