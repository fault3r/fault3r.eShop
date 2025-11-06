
using System;
using AccountService.Domain.Abstractions;
using AccountService.Domain.Common;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates.Account.Events;

public sealed class AccountSignedUpDomainEvent : DomainEvent
{
    public Identity AccountId { get; }
    public Email Email { get; }
    public DateTime SignedUpAt { get; }

    public AccountSignedUpDomainEvent(Identity accountId, Email email)
    {
        AccountId = accountId;
        Email = email;
        SignedUpAt = DateTime.UtcNow;
    }
}
