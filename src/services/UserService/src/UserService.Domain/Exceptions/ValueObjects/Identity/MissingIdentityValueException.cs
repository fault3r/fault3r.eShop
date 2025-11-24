
using System;

namespace UserService.Domain.Exceptions.ValueObjects.Identity;

public class MissingIdentityValueException : DomainException
{
    public MissingIdentityValueException() : base("Identity value is required") { }

}
