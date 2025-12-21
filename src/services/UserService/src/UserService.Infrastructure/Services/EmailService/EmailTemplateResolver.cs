
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class EmailTemplateResolver : IEmailTemplateResolver
{
    private readonly Dictionary<EmailTemplateType, string> templates = [];

    public EmailTemplateResolver(string templatesPath)
    {
        if (string.IsNullOrWhiteSpace(templatesPath))
            throw new MissingTemplatesPathException();

        var values = Enum.GetValues<EmailTemplateType>();

        foreach (var item in values)
        {
            var path = Path.Combine(templatesPath, $"{item}.cshtml");

            if (!File.Exists(path))
                throw new MissingTemplateFileException();

            templates.Add(
                key: item,
                value: path
            );
        }
    }

    public async Task<string> ResolveAsync(
        EmailTemplateType templateType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var path = templates[templateType];
            return await File.ReadAllTextAsync(path, cancellationToken);
        }
        catch { throw new EmailTemplateResolveException(); }
    }
}