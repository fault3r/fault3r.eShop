
using System;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public class FluentEmailRazorBodyRenderer : IEmailBodyMaker
{
    private readonly RazorRenderer razorRenderer = new();

    public async Task<string> RenderRazorBody(string razorTemplatePath, object model, CancellationToken cancellationToken = default)
    {
        var template = await File.ReadAllTextAsync(razorTemplatePath, cancellationToken);
        return await razorRenderer.ParseAsync(template, model, isHtml: true);
    }
}
