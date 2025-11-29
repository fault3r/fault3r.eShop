
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User.Events;

public class UserCreatedDomainEvent : DomainEvent
{
    public Identity AccountId { get; }
    public Email Email { get; }

    

}
