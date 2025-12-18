
using System;
using UserService.Domain.Exceptions.Abstraction.AggregateRoot;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract class AggregateRoot<T, TId>
    : Entity<T, TId>, IAggregateRoot, IAggregateRoot<TId>
    where T : AggregateRoot<T, TId>
{
    private readonly List<IDomainEvent> domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => domainEvents.AsReadOnly();    

    protected AggregateRoot(TId id) : base(id) { }

    protected void RaiseEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is null)
            throw new MissingDomainEventException();

        domainEvents.Add(domainEvent);
    }

    public void ClearEvents()
        => domainEvents.Clear();

    public override string ToString()
        => $"{GetType().Name}:{base.ToString()}";
}