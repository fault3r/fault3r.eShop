
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Factories;

public sealed class UserFactory
{
    public static User Create(Email email, PasswordHash passwordHash, FullName fullName)
    {
        var defaultId = Identity.New();
        var defaultRole = Role.User;
        var defaultStatus = Status.Pending;

        return User.Create(
            id: defaultId,
            email: email,
            passwordHash: passwordHash,
            fullName: fullName,
            role: defaultRole,
            status: defaultStatus
        );
    }

    public static Result<User> TryCreate(Email email, PasswordHash passwordHash, FullName fullName)
    {
        try
        {
            var user = Create(email, passwordHash, fullName);
            return Result<User>.Success(user);
        }
        catch (DomainException ex)
        {
            return Result<User>.Failure(ex.Message);
        }
    }

    public static User From(
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