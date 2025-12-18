
using System;

namespace UserService.Domain.Interfaces;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}