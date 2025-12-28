
using System;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class FluentEmailRazorBodyRenderer : IEmailBodyRenderer
{
    private readonly RazorRenderer renderer = new();

    public async Task<string> RenderAsync(
        string template,
        object model,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(template);
        ArgumentNullException.ThrowIfNull(model);

        return await renderer.ParseAsync(template, model, isHtml: true);
    }
}
