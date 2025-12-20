
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class EmailTemplateResolver : IEmailTemplateResolver
{
    public readonly Dictionary<EmailTemplateType, string> Templates = [];

    public EmailTemplateResolver(string templatesPath)
    {
        if (string.IsNullOrWhiteSpace(templatesPath))
            throw new MissingTemplatesPathException();

        SeedTemplates(templatesPath);
    }    
    
    private void  SeedTemplates(string templatesPath)
    {
        Templates.Add(
            key: EmailTemplateType.Welcome,
            value: Path.Combine(templatesPath, $"{nameof(EmailTemplateType.Welcome)}.cshtml")
        );
    }

    public async Task<string> ResolveAsync(
        EmailTemplateType templateType,
        CancellationToken cancellationToken = default)
    {
        var templatePath = Templates[templateType];
        return await File.ReadAllTextAsync(templatePath, cancellationToken);
    }

    public async Task<string> GetWelcome(CancellationToken cancellationToken = default)
        => await ResolveAsync(EmailTemplateType.Welcome, cancellationToken);
}