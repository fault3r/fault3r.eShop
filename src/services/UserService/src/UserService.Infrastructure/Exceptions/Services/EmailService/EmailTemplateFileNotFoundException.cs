
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class EmailTemplateFileNotFoundException : InfrastructureException
{
    public EmailTemplateFileNotFoundException()
        : base("email template file not found") { }
}
