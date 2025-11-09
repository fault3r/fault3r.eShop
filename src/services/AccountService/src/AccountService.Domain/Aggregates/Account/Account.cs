
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.ValueObjects;
using AccountService.Domain.Aggregates.Account.Events;
using AccountService.Domain.Exceptions.Email;
using AccountService.Domain.Exceptions.FullName;
using AccountService.Domain.Exceptions.PasswordHash;
using AccountService.Domain.Exceptions.Role;
using AccountService.Domain.Exceptions.Status;

namespace AccountService.Domain.Aggregates.Account;

public sealed class Account : AggregateRoot
{
    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public Status Status { get; private set; }

    private Account(Identity id, string fullName, Email email, string passwordHash, Role role, Status status)
        : base(id)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new MissingFullNameException();
        if (email is null)
            throw new MissingEmailException();
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new MissingPasswordHashException();
        if (role is null)
            throw new MissingRoleException();
        if (status is null)
            throw new MissingStatusException();
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        Status = status;
    }

    internal static Account Create(Identity id, string fullName, Email email, string passwordHash, Role role, Status status)
    {
        var account = new Account(id, fullName, email, passwordHash, role, status);
        account.RaiseEvent(new AccountCreatedDomainEvent(account.Id, account.Email));
        return account;
    }
}