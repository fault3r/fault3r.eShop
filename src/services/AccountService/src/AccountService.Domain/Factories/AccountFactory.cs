
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Factories;

public static class AccountFactory
{
    public static Account Register(string fullName, string email, string passwordHash)
    {
        return Account.Register(
            id: Identity.New(),
            fullName: fullName,
            email: new Email(email),
            passwordHash: passwordHash,
            role: Role.User,
            status: Status.Pending);
    }
}