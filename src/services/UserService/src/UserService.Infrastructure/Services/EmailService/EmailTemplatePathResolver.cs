
using System;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public class EmailTemplatePathResolver : IEmailTemplatePathResolver
{
    public readonly Dictionary<EmailTemplateType, string> AllTemplates = new()
    {
        {EmailTemplateType.welcome, "Services/EmailService/Templates/welcome.cshtml"}
    };

    public async Task<string> ResolveAsync(EmailTemplateType emailTemplateType)
    {
        if(!AllTemplates.TryGetValue(emailTemplateType, out var path))
            throw new MissingEmailTemplateException();

        return path;
    }
}