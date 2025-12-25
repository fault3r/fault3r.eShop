
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public class EmailSenderArgumentException : InfrastructureException
{
    public EmailSenderArgumentException() : base("email sender arguments are required") { }
}
