
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class MissingTemplateFileException : InfrastructureException
{
    public MissingTemplateFileException()
        : base("email temaplate file does not exist") { }
}
