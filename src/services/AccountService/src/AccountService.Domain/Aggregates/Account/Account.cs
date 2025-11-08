
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Aggregates.Account.Events;
using AccountService.Domain.Common;
using AccountService.Domain.Exceptions;
using AccountService.Domain.Exceptions.Email;
using AccountService.Domain.Exceptions.Role;
using AccountService.Domain.Exceptions.Status;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account;

public class Account : AggregateRoot
{
    public string FullName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public Role Role { get; private set; }
    public Status Status { get; private set; }

    protected Account() : base() { } // EF core

    private Account(Identity id, string fullName, Email email, string passwordHash, Role role, Status status)
    {
        if (string.IsNullOrEmpty(fullName))
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

    public static Account RegisterNew(Identity id, string fullName, Email email, string passwordHash, Role role, Status status)
    {
        var account = new Account(id, fullName, email, passwordHash, role, status);
        account.RaiseEvent(new AccountSignedUpDomainEvent(id, email));
        return account;
    }

    public void PromoteToAdmin()
    {
        if (Role == Role.Admin) return;
        Role = Role.Admin;
        //RaiseEvent();
    }

    public void DemoteToUser()
    {
        if (Role == Role.User) return;
        Role = Role.User;
        //RaiseEvent();
    }

}