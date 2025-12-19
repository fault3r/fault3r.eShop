
using System;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public class FluentEmailBodyMaker : IEmailBodyMaker
{
    private readonly RazorRenderer razorRenderer = new();

    public static readonly Dictionary<EmailTemplateType, string> AllTemplates = new()
    {
        {EmailTemplateType.Welcome, "Services/EmailService/Templates/welcome.cshtml" },
    };

    public string ResolveTemplatePath(EmailTemplateType templateType)
    {
        if (!AllTemplates.TryGetValue(templateType, out var path))
            throw new Exception();  //exception

        return path;
    }

    public async Task<string> RenderRazorBody(string razorTemplatePath, object model, CancellationToken cancellationToken = default)
    {
        var template = await File.ReadAllTextAsync(razorTemplatePath, cancellationToken);
        return await razorRenderer.ParseAsync(template, model, isHtml: true);
    }
}
