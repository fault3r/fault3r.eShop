
using System;

namespace UserService.Domain.Abstractions;

public abstract class AggregateRoot<TType, TId> : Entity<TType, TId>
    where TType : AggregateRoot<TType, TId>
{
    protected AggregateRoot(TId id) : base(id) { }
}
