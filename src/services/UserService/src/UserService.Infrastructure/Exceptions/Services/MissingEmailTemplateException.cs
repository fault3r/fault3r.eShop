
using System;

namespace UserService.Infrastructure.Exceptions.Services;

public class MissingEmailTemplateException
{
    public MissingEmailTemplateException() : base("email template is required") { }
}