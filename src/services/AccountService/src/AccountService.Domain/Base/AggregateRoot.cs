
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.Common;

namespace AccountService.Domain.Base;

public abstract class AggregateRoot : Entity
{
    private readonly IList<DomainEvent> events = [];

    public ReadOnlyCollection<DomainEvent> Events
        => events.AsReadOnly();

    protected AggregateRoot() : base() { }

    protected AggregateRoot(Identity id) : base(id) { }

    protected void AddDomainEvent(DomainEvent @event)
        => events.Add(@event);

    public void ClearEvents()
        => events.Clear();
}
