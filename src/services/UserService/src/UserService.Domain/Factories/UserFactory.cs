
using System;
using UserService.Domain.Aggregates;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Factories;

public sealed class UserFactory
{
    public static User CreateNew(Email email, PasswordHash passwordHash, FullName fullName)
        => User.Create(Identity.New(), email, passwordHash, fullName, Role.User, Status.Pending);

    public static User CreateNew(
        Identity id,
        Email email,
        PasswordHash passwordHash,
        FullName fullName,
        Role role,
        Status status)
    {
        return User.Create(id, email, passwordHash, fullName, role, status);
    }
}