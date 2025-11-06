
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
        : base(id)
    {
        if (string.IsNullOrEmpty(fullName))
            throw new DomainException("FullName is required");
        if (email is null)
            throw new DomainException("Email is required");
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("PasswordHash is required");
        if (role is null)
            throw new DomainException("Role is required");
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        RaiseEvent(new AccountSignedUpDomainEvent(id, email));
    }

    public static Account SignUp(string fullName, Email email, string password, Role role)
    {
        var passwordHash = password;
        return new Account(Identity.New(), fullName, email, passwordHash, role);
    }
}