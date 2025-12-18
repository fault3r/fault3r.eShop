
using System;

namespace UserService.Domain.Interfaces;

public interface IAggregateRoot { }

public interface IAggregateRoot<TId>
{
    TId Id { get; }
}

