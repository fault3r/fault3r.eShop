
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Aggregates.Account.Events;
using AccountService.Domain.Common;
using AccountService.Domain.Exceptions;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account;

public class Account : AggregateRoot
{
    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }

    protected Account() : base() { } // EF core

    private Account(Identity id, string fullName, Email email, string passwordHash, Role role)
    {
        if (string.IsNullOrEmpty(fullName))
            throw new MissingFullNameException();
        if (email is null)
            throw new MissingEmailException();
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new MissingPasswordHashException();
        if (role is null)
            throw new MissingRoleException();
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        RaiseEvent(new AccountSignedUpDomainEvent(id, email));
    }

    internal static Account CreateNew(Identity id, string fullName, Email email, string passwordHash, Role role)
    {
        return new Account(id, fullName, email, passwordHash, role);
    }
}