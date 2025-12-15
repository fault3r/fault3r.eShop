
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.Exceptions.ValueObjects.Role;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;
public sealed record UserRoleChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email Email { get; init; }    
    public Role NewRole { get; init; }

    public UserRoleChangedEvent(
        Identity userId,
        Email email,
        Role newRole,
        Guid? eventId = null,
        DateTime? occurredOn = null)
        : base(eventId, occurredOn)
    {
        UserId = userId
            ?? throw new MissingIdentityException();
        Email = email
            ?? throw new MissingEmailException();
        NewRole = newRole
            ?? throw new MissingRoleException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewRole={NewRole}";
}
