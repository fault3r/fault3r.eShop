
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class EmailTemplateResolver : IEmailTemplateResolver
{
    public readonly Dictionary<EmailTemplateType, string> Templates = [];

    public EmailTemplateResolver(string rootPath, string templatesPath)
    {
        if (string.IsNullOrWhiteSpace(rootPath))
            throw new MissingRootPathException();

        if (string.IsNullOrWhiteSpace(templatesPath))
            throw new MissingTemplatePathException();

        SeedTemplates(Path.Combine(rootPath, templatesPath));
    }    
    
    public void  SeedTemplates(string path)
    {
        Templates.Add(
            key: EmailTemplateType.Welcome,
            value: Path.Combine(path, $"{nameof(EmailTemplateType.Welcome)}.cshtml")
        );
    }

    public async Task<string> ResolveAsync(
        EmailTemplateType templateType,
        CancellationToken cancellationToken = default)
    {
        bool hasTemplate = Templates
            .TryGetValue(templateType, out var templatePath);

        if (!hasTemplate || string.IsNullOrWhiteSpace(templatePath))
            throw new InvalidEmailTemplateException(templateType.ToString());

        return await File.ReadAllTextAsync(templatePath, cancellationToken);
    }

    public async Task<string> GetWelcome(CancellationToken cancellationToken = default)
        => await ResolveAsync(EmailTemplateType.Welcome, cancellationToken);
}