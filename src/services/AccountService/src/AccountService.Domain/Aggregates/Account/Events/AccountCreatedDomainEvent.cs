
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account.Events;

public sealed class AccountCreatedDomainEvent : DomainEvent
{
    public Identity Id { get; }
    public Email Email { get; }

    public AccountCreatedDomainEvent(Identity id, Email email)
    {
        Id = id;
        Email = email;        
    }
}
