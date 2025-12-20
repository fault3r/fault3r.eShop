
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class EmailTemplateResolver(string contentRoot) : IEmailTemplateResolver
{
    private readonly string rootPath = contentRoot;
    
    public readonly Dictionary<EmailTemplateType, string> Templates = new()
    {
        {EmailTemplateType.Welcome, "UserService.Infrastructure/Services/EmailService/Templates/welcome.cshtml"}
    };

    public async Task<string> ResolveAsync(
        EmailTemplateType templateType,
        CancellationToken cancellationToken = default)
    {
        bool hasTemplate = Templates
            .TryGetValue(templateType, out var templatePath);

        if (!hasTemplate || string.IsNullOrWhiteSpace(templatePath))
            throw new InvalidEmailTemplateException(templateType.ToString());

        string fullPath = Path.Combine(rootPath, templatePath);
        return await File.ReadAllTextAsync(fullPath, cancellationToken);
    }

    public async Task<string> GetWelcome(CancellationToken cancellationToken = default)
        => await ResolveAsync(EmailTemplateType.Welcome, cancellationToken);
}