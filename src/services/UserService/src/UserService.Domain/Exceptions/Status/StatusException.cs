
using System;

namespace UserService.Domain.Exceptions.Status;

public class StatusException(
    string message
) : DomainException(message) { }
