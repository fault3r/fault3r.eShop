using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Exceptions.Email;
using AccountService.Domain.Exceptions.Identity;
using AccountService.Domain.Interfaces;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account.Events;

public sealed class AccountConfirmedDomainEvent : DomainEvent, IDomainEvent
{
    public Identity AccountId { get; }
    public Email Email { get; }

    public AccountConfirmedDomainEvent(Identity accountId, Email email)
    {
        AccountId = accountId
            ?? throw new MissingIdentityException();
        Email = email
            ?? throw new MissingEmailException();
    }
}
