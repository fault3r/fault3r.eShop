
using System;
using UserService.Domain.Abstractions;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Aggregates.User.Events;

public sealed record UserFullNameChangedEvent : DomainEvent
{
    public Identity UserId { get; init; }
    public FullName NewFullName { get; init; }

    public UserFullNameChangedEvent(Identity userId, FullName newFullName)
        : base()
    {
        UserId = userId;
        NewFullName = newFullName;
    }

    public UserFullNameChangedEvent(
        Guid eventId,
        DateTime occurredOn,
        Identity userId,
        FullName newFullName)
        : base(eventId, occurredOn)
    {
        UserId = userId;
        NewFullName = newFullName;
    }

    public override string ToString()
        => $"{base.ToString()} | UserId={UserId}, NewFullName={NewFullName}";
}
