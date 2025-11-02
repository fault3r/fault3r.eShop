
using System;

namespace AccountService.Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> Events
        => _events.AsReadOnly();

    protected void RaiseEvent(IDomainEvent @event)
        => _events.Add(@event); 
      
    public void ClearEvents()
        => _events.Clear();
}
