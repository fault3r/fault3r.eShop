
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class EmailSenderException : InfrastructureException
{
    public EmailSenderException() : base("cannot send email") { }
}
