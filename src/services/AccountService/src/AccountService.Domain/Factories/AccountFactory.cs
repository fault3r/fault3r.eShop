
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Factories;

public sealed class AccountFactory
{
    public static Account CreateNew(string fullName, string email, string passwordHash)
        => Account.Create(
            id: Identity.New(),
            fullName: fullName,
            email: Email.From(email),
            passwordHash: passwordHash,
            role: Role.User,
            status: Status.Pending);
}