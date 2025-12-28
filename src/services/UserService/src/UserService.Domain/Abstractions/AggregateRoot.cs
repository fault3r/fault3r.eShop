
using System;
using UserService.Domain.Interfaces;

namespace UserService.Domain.Abstractions;

public abstract class AggregateRoot<T, TId>
    : Entity<T, TId>, IAggregateRoot, IAggregateRoot<TId>
    where T : AggregateRoot<T, TId>
{
    private readonly List<IDomainEvent> events = [];

    public IReadOnlyCollection<IDomainEvent> Events
        => [.. events];      

    protected AggregateRoot(TId id) : base(id) { }

    protected void RaiseEvent(IDomainEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        events.Add(@event);
    }

    public void ClearEvents()
        => events.Clear();

    public override string ToString()
        => $"{GetType().Name}:{base.ToString()}";
}