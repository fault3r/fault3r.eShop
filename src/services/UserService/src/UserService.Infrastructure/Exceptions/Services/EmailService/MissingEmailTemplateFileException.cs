
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingEmailTemplateFileException : InfrastructureException
{
    public MissingEmailTemplateFileException()
        : base("email temaplate file does not exist") { }
}
 