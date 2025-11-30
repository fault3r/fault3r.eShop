
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User.Events;

public sealed record UserCreatedDomainEvent(
    Identity UserId, Email UserEmail) : DomainEvent;