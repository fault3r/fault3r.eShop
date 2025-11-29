
using System;
using UserService.Domain.Exceptions.Abstraction.AggregateRoot;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract class AggregateRoot<TType, TId> : Entity<TType, TId>
    where TType : AggregateRoot<TType, TId>
{
    private readonly List<IDomainEvent> domainEvents = [];

    protected AggregateRoot(TId id) : base(id) { }

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => domainEvents.AsReadOnly();

    protected void RaiseEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new MissingDomainEventException();

        domainEvents.Add(domainEvent);
    }

    public void ClearEvents()
        => domainEvents.Clear();
}
