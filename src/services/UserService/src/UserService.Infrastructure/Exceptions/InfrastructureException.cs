
using System;

namespace UserService.Infrastructure.Exceptions;

public class InfrastructureException(
    string message
) : Exception(message) { }