
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account.Events;

public sealed class AccountCreatedDomainEvent : DomainEvent
{
    public Identity AccountId { get; }
    public Email Email { get; }

    public AccountCreatedDomainEvent(Identity accountId, Email email)
    {
        AccountId = accountId;
        Email = email;
    }
}
