
using System;
using AccountService.Domain.Aggregates.Account;
using AccountService.Domain.Common;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Factories;

public sealed class AccountFactory
{
    public static Account CreateNew(
        string fullName,
        string email,
        string passwordHash,
        Identity? id = null,
        Role? role = null,
        Status? status = null)
    {
        return Account.Create(
            id: id ?? Identity.New(),
            fullName: fullName,
            email: Email.From(email),
            passwordHash: passwordHash,
            role: role ?? Role.User,
            status: status ?? Status.Pending);
    }
}