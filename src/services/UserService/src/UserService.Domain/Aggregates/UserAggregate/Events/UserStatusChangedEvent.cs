
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.Exceptions.ValueObjects.Email;
using UserService.Domain.Exceptions.ValueObjects.Identity;
using UserService.Domain.Exceptions.ValueObjects.Status;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.UserAggregate.Events;

public sealed record UserStatusChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public Email Email { get; init; }
    public Status NewStatus { get; init; }

    public UserStatusChangedEvent(
        Identity userId,
        Email email,
        Status status,
        Guid? eventId = null,
        DateTime? occurredOn = null)
        : base(eventId, occurredOn)
    {
        UserId = userId
            ?? throw new MissingIdentityException();

        Email = email
            ?? throw new MissingEmailException();

        NewStatus = status
            ?? throw new MissingStatusException();
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, Email={Email}, NewStatus={NewStatus}";
}
