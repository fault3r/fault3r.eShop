
using System;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public class FluentEmailRazorBodyRenderer : IEmailBodyRenderer
{
    private readonly RazorRenderer razorRenderer = new();

    public async Task<string> RenderAsync(
        string templatePath,
        object model,
        CancellationToken cancellationToken = default)
    {
        var template = await File.ReadAllTextAsync(templatePath, cancellationToken);
        return await razorRenderer.ParseAsync(template, model, isHtml: true);
    }
}
