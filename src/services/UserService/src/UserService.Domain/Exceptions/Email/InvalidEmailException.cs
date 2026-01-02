
using System;

namespace UserService.Domain.Exceptions.Email;

public sealed class InvalidEmailException(
    string value
) : EmailException($"invalid email: {value}") { }