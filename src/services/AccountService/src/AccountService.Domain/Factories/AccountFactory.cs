
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Factories;

public static class AccountFactory
{
    public static Account Create(string fullName, string email, string passwordHash, string role = "User")
    {
        return Account.CreateNew(
            id: Identity.New(),
            fullName: fullName,
            email: new Email(email),
            passwordHash: passwordHash,
            role: Role.From(role));
    }
}