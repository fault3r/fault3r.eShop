
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User.Events;

public sealed record UserFullNameChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email Email { get; init; }
    public FullName NewFullName { get; init; }

    public UserFullNameChangedEvent(
        Identity userId,
        Email email,
        FullName newFullName,
        Guid? eventId = null,
        DateTime? occurredOn = null)
        : base(eventId, occurredOn)
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
