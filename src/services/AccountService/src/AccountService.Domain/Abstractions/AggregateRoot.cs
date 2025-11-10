
using System;
using System.Collections.ObjectModel;
using AccountService.Domain.Exceptions.DomainEvent;
using AccountService.Domain.Interfaces;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Abstractions;

public abstract class AggregateRoot : Entity
{
    private readonly IList<IDomainEvent> events = [];

    public ReadOnlyCollection<IDomainEvent> DomainEvents
        => events.AsReadOnly();

    protected AggregateRoot(Identity id) : base(id) { }

    public void RaiseEvent(IDomainEvent @event)
        => events.Add(@event
            ?? throw new MissingDomainEventException());

    public void ClearEvents()
        => events.Clear();
}