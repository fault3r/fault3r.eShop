
using System;

namespace UserService.Domain.Exceptions.Status;

public sealed class UnsupportedStatusException(
    string value
) : StatusException($"unsupported status: {value}") { }
