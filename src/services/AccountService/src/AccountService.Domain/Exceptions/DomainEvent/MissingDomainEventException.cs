
using System;

namespace AccountService.Domain.Exceptions.DomainEvent;

public class MissingDomainEventException : DomainException
{
    public MissingDomainEventException() : base("DomainEvent is required") { }
}
