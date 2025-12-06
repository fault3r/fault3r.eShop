
using System;

namespace UserService.Domain.Interfaces;

public interface IDomainEvent
{
    Guid EventId { get; init; }
    DateTime OccurredOn { get; init; }
}