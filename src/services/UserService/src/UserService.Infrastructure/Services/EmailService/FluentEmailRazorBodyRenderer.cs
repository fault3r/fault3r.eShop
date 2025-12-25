
using System;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class FluentEmailRazorBodyRenderer : IEmailBodyRenderer
{
    private readonly RazorRenderer renderer = new();

    public async Task<string> RenderAsync(
        string template,
        object model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await renderer.ParseAsync(template, model, isHtml: true);
        }
        catch { throw new EmailBodyRenderException(); }
    }
}
