
using System;

namespace UserService.Domain.Exceptions.Abstraction.AggregateRoot;

public class MissingDomainEventException : DomainException
{
    public MissingDomainEventException() : base("domain event is required"){}
}
