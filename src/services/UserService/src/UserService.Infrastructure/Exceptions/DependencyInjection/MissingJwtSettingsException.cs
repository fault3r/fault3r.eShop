
using System;

namespace UserService.Infrastructure.Exceptions.DependencyInjection;

public sealed class MissingJwtSettingsException : InfrastructureException
{
    public MissingJwtSettingsException()
        : base("missing json web token settings") { }
}
