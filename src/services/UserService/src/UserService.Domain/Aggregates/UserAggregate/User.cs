
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Aggregates.UserAggregate.Events;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate;

public sealed class User : AggregateRoot<User, Identity>
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
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(passwordHash);
        ArgumentNullException.ThrowIfNull(fullName);
        ArgumentNullException.ThrowIfNull(role);
        ArgumentNullException.ThrowIfNull(status);

        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        Status = status;
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
        user.RaiseEvent(new UserCreatedEvent(user.Id, user.Email, user.FullName));
        return user;
    }

    public void ChangeEmail(Email newEmail)
    {
        ArgumentNullException.ThrowIfNull(newEmail);

        if (newEmail == Email)
            return;

        Email = newEmail;
        RaiseEvent(new UserEmailChangedEvent(Id, Email, FullName));
    }

    public void ChangePassword(PasswordHash newPasswordHash)
    {
        ArgumentNullException.ThrowIfNull(newPasswordHash);

        if (newPasswordHash == PasswordHash)
            return;

        PasswordHash = newPasswordHash;
        RaiseEvent(new UserPasswordChangedEvent(Id, Email, FullName));
    }

    public void ChangeFullName(FullName newFullName)
    {
        ArgumentNullException.ThrowIfNull(newFullName);

        if (newFullName == FullName)
            return;

        FullName = newFullName;
        RaiseEvent(new UserFullNameChangedEvent(Id, Email, FullName));
    }

    public void ChangeRole(Role newRole)
    {
        ArgumentNullException.ThrowIfNull(newRole);

        if (newRole == Role)
            return;

        Role = newRole;
        RaiseEvent(new UserRoleChangedEvent(Id, Email, FullName, Role));
    }

    public void ChangeStatus(Status newStatus)
    {
        ArgumentNullException.ThrowIfNull(newStatus);

        Status = newStatus;
        RaiseEvent(new UserStatusChangedEvent(Id, Email, FullName, Status));
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