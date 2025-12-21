
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

        var enumValues = Enum.GetValues<EmailTemplateType>();

        foreach (var item in enumValues)
        {
            Templates.Add(
                key: item,
                value: Path.Combine(templatesPath, $"{item}.cshtml")
            );
        }
    }

    public async Task<string> ResolveAsync(
        EmailTemplateType templateType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var templatePath = Templates[templateType];
            return await File.ReadAllTextAsync(templatePath, cancellationToken);
        }
        catch { throw new EmailTemplateResolveException(); }
    }

    public async Task<string> GetWelcome(CancellationToken cancellationToken = default)
        => await ResolveAsync(EmailTemplateType.Welcome, cancellationToken);
}