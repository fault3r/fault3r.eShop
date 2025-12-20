
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingTemplatesPathException : InfrastructureException
{
    public MissingTemplatesPathException()
        : base("missing templates path") { }
}
