
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingRootPathException : InfrastructureException
{
    public MissingRootPathException()
        : base("root path is required") { }
}
