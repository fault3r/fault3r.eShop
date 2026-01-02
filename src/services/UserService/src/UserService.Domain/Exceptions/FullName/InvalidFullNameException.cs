
using System;

namespace UserService.Domain.Exceptions.FullName;

public sealed class InvalidFullNameException(
    string value
) : FullNameException($"invalid fullname: {value}") { }
