
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    private readonly IList<DomainEvent> events = [];

    public ReadOnlyCollection<DomainEvent> Events
        => events.AsReadOnly();

    protected AggregateRoot() : base() { }

    protected AggregateRoot(Identity id) : base(id) { }

    protected void RaiseEvent(DomainEvent @event)
        => events.Add(@event);

    public void ClearEvents()
        => events.Clear();
}
