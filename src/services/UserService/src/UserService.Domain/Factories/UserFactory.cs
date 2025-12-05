
using System;
using UserService.Domain.Aggregates;
using UserService.Domain.Security;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Factories;

public sealed class UserFactory
{
    private readonly IPasswordHasher _passwordHasher;

    public UserFactory(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public User Create(
        Email email,
        string rawPassword,
        FullName fullName,
        Identity? id,
        Role? role,
        Status? status)
    {
        if (string.IsNullOrWhiteSpace(rawPassword))
            throw new ArgumentException("Password cannot be empty.", nameof(rawPassword));

        var passwordHash = _passwordHasher.Hash(rawPassword);

        return User.Create(
            id ?? Identity.New(),
            email,
            passwordHash,
            fullName,
            role ?? Role.User,
            status ?? Status.Pending);
    }
}
