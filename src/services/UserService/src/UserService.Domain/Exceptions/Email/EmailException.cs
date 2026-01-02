
using System;

namespace UserService.Domain.Exceptions.Email;

public class EmailException(
    string message
) : DomainException(message) { }