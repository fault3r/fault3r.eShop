
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class RazorEmailTemplateResolver : IEmailTemplateResolver
{
    private readonly Dictionary<EmailTemplateType, string> templates = [];

    public RazorEmailTemplateResolver(string templatesPath)
    {
        ArgumentException.ThrowIfNullOrEmpty(templatesPath);

        var allTemplates = Enum.GetValues<EmailTemplateType>();

        foreach (var template in allTemplates)
        {
            var path = Path.Combine(templatesPath, $"{template}.cshtml");

            if (!File.Exists(path))
                throw new EmailTemplateFileNotFoundException();

            templates.Add(
                key: template,
                value: path
            );
        }
    }

    public async Task<string> ResolveAsync(
        EmailTemplateType emailTemplateType,
        CancellationToken cancellationToken = default)
    {
        var path = templates[emailTemplateType];
        return await File.ReadAllTextAsync(path, cancellationToken);
    }
}