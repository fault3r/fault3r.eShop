
using System;

namespace UserService.Domain.Interfaces;

public interface IValueObject { }

public interface IValueObject<TValue>
{
    public abstract TValue Value { get; }
}
