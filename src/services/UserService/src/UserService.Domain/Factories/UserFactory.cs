
using System;
using UserService.Domain.Aggregates.UserAggregate;
using UserService.Domain.Common;
using UserService.Domain.Exceptions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Factories;

public sealed class UserFactory
{
    public static User CreateNew(Email email, PasswordHash passwordHash, FullName fullName)
        => User.Create(Identity.New(), email, passwordHash, fullName, Role.User, Status.Pending);

    public static Result<User> TryCreateNew(Email email, PasswordHash passwordHash, FullName fullName)
    {
        try
        {
            var user = CreateNew(email, passwordHash, fullName);
            return Result<User>.Success(user);
        }
        catch (DomainException ex)
        {
            return Result<User>.Failure(
                $"Cannot create user! : {ex.Message}");
        }
    }

    public static User Create(
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