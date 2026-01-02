
using System;

namespace UserService.Domain.Exceptions.Identity;

public sealed class InvalidIdentityException(
    string value
) : IdentityException($"invalid identity: {value}") { }
