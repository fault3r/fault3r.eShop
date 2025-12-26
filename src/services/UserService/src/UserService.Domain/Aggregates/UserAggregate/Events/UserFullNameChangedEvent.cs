
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.Exceptions.ValueObjects.Identity;
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
        UserId = userId
            ?? throw new MissingIdentityException();

        Email = email
            ?? throw new MissingEmailException();

        NewFullName = newFullName
            ?? throw new MissingFullNameException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewFullName={NewFullName}";
}
