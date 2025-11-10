using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Email;
using AccountService.Domain.Exceptions.FullName;
using AccountService.Domain.Exceptions.Identity;
using AccountService.Domain.Interfaces;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account.Events;

public sealed class AccountFullNameChangedDomainEvent : DomainEvent, IDomainEvent
{
    public Identity AccountId { get; }
    public Email Email { get; }
    public string FullName { get; }

    public AccountFullNameChangedDomainEvent(Identity accountId, Email email, string fullName)
    {
        AccountId = accountId
            ?? throw new MissingIdentityException();
        Email = email
            ?? throw new MissingEmailException();
        if (string.IsNullOrWhiteSpace(fullName))
            throw new MissingFullNameException();
        else
            FullName = fullName;
    }
}
