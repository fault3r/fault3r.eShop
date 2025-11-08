using System;

namespace AccountService.Domain.Exceptions.Status;

public class MissingStatusException : DomainException
{
    public MissingStatusException() : base($"Status value is required") { }
}
