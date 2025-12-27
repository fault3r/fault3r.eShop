
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingEmailTemplatesPathException : InfrastructureException
{
    public MissingEmailTemplatesPathException()
        : base("missing templates path") { }
}
