
using System;

namespace UserService.Domain.Exceptions.FullName;

public class FullNameException(
    string message
) : DomainException(message) { }
