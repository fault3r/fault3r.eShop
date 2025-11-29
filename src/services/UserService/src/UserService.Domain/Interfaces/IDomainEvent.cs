
using System;

namespace UserService.Domain.Interfaces;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
