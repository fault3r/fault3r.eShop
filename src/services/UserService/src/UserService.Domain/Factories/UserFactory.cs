
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Factories;

public sealed class UserFactory
{
    public static User CreateNew(Email email, PasswordHash passwordHash, FullName fullName)
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

    public static Result<User> TryCreateNew(Email email, PasswordHash passwordHash, FullName fullName)
    {
        try
        {
            var user = CreateNew(email, passwordHash, fullName);
            return Result<User>.Success(user);
        }
        catch (DomainException exception)
        {
            return Result<User>.Failure(exception.Message);
        }
    }

    public static User CreateFrom(
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