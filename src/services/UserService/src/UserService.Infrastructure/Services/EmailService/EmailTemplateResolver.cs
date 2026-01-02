
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class EmailTemplateResolver : IEmailTemplateResolver
{
    private readonly Dictionary<EmailTemplateType, string> templates = [];

    public EmailTemplateResolver(string templatesPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(templatesPath);

        var allTemplates = Enum.GetValues<EmailTemplateType>();

        foreach (var template in allTemplates)
        {
            var path = Path.Combine(templatesPath, $"{template}.cshtml");

            if (!File.Exists(path))
                throw new EmailTemplateFileNotFoundException();

            templates.Add(template, path);
        }
    }

    public async Task<string> ResolveAsync(
        EmailTemplateType emailTemplateType,
        CancellationToken ct = default)
    {
        var path = templates[emailTemplateType];

        return await File.ReadAllTextAsync(path, ct);
    }
}