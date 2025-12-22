
using System;

namespace UserService.Domain.Interfaces;

public interface IEntity { }

public interface IEntity<TId>
{
    TId Id { get; }
}
