
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.FullName;
using UserService.Domain.Exceptions.ValueObjects.PasswordHash;
using UserService.Domain.Exceptions.ValueObjects.Role;
using UserService.Domain.Exceptions.ValueObjects.Status;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate;

public class User : AggregateRoot<User, Identity>
{
    public Email Email { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public FullName FullName { get; private set; }
    public Role Role { get; private set; }
    public Status Status { get; private set; }

    private User(
        Identity id,
        Email email,
        PasswordHash passwordHash,
        FullName fullName,
        Role role,
        Status status)
        : base(id)
    {
        Email = email
            ?? throw new MissingEmailException();

        PasswordHash = passwordHash
            ?? throw new MissingPasswordHashException();
            
        FullName = fullName
            ?? throw new MissingFullNameException();

        Role = role
            ?? throw new MissingRoleException();

        Status = status
            ?? throw new MissingStatusException();
    }

    internal static User Create(
        Identity id,
        Email email,
        PasswordHash passwordHash,
        FullName fullName,
        Role role,
        Status status)
    {
        var user = new User(id, email, passwordHash, fullName, role, status);
        user.RaiseEvent(new UserCreatedEvent(user.Id, user.Email));
        return user;
    }

    public void ChangeEmail(Email newEmail)
    {
        if (newEmail is null)
            throw new MissingEmailException();

        if (newEmail == Email)
            return;

        Email = newEmail;
        RaiseEvent(new UserEmailChangedEvent(Id, Email));
    }

    public void ChangePassword(PasswordHash newPasswordHash)
    {
        if (newPasswordHash is null)
            throw new MissingPasswordHashException();

        if (newPasswordHash == PasswordHash)
            return;

        PasswordHash = newPasswordHash;
        RaiseEvent(new UserPasswordChangedEvent(Id, Email));
    }

    public void ChangeFullName(FullName newFullName)
    {
        if (newFullName is null)
            throw new MissingFullNameException();

        if (newFullName == FullName)
            return;

        FullName = newFullName;
        RaiseEvent(new UserFullNameChangedEvent(Id, Email, FullName));
    }

    public void ChangeRole(Role newRole)
    {
        if (newRole is null)
            throw new MissingRoleException();

        if (newRole == Role)
            return;

        Role = newRole;
        RaiseEvent(new UserRoleChangedEvent(Id, Email, Role));
    }

    public void Activate()
    {
        if (Status.IsActive)
            return;

        Status = Status.Active;
        RaiseEvent(new UserActivatedEvent(Id, Email));
    }

    public void Lock()
    {
        if (Status.IsLocked)
            return;

        Status = Status.Locked;
        RaiseEvent(new UserLockedEvent(Id, Email));
    }

    // EFCore
    public User(Identity id) : base(id)
    {
        Email = null!;
        PasswordHash = null!;
        FullName = null!;
        Role = null!;
        Status = null!;
    }
}