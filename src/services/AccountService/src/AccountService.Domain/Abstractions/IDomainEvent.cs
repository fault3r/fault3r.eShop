
using System;

namespace AccountService.Domain.Abstractions;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
