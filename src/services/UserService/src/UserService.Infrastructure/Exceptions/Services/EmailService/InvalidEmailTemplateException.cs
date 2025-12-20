
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class InvalidEmailTemplateException : InfrastructureException
{
    public InvalidEmailTemplateException(string template)
        : base($"invalid email template type: {template}") { }
}