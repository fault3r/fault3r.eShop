
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.Exceptions.ValueObjects.Identity;
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
        UserId = userId
            ?? throw new MissingIdentityException();

        NewEmail = newEmail
            ?? throw new MissingEmailException();

        FullName = fullName
            ?? throw new MissingFullNameException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, NewEmail={NewEmail}, FullName={FullName}";
}
