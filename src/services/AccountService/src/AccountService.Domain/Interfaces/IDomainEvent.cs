
using System;

namespace AccountService.Domain.Interfaces;

public interface IDomainEvent
{
    Guid Id { get; }
    
    DateTime OccurredOn { get; }
}

