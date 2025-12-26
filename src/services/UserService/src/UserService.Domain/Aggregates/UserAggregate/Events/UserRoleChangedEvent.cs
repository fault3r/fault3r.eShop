
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.Exceptions.ValueObjects.Role;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserRoleChangedEvent : DomainEvent
{
    public Identity UserId { get; }
    public Email Email { get; }
    public FullName FullName { get; }
    public Role NewRole { get; }

    public UserRoleChangedEvent(
        Identity userId,
        Email email,
        FullName fullName,
        Role newRole)
    {
        UserId = userId
            ?? throw new MissingIdentityException();

        Email = email
            ?? throw new MissingEmailException();

        FullName = fullName
            ?? throw new MissingFullNameException();
        
        NewRole = newRole
            ?? throw new MissingRoleException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, FullName={FullName}, NewRole={NewRole}";
}
