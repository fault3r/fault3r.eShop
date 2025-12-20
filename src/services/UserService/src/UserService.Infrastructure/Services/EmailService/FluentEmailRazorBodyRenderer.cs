
using System;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public class FluentEmailRazorBodyRenderer : IEmailBodyRenderer
{
    private readonly RazorRenderer renderer = new();

    public async Task<string> RenderAsync(
        string template,
        object model,
        CancellationToken cancellationToken = default)
    {
        return await renderer.ParseAsync(template, model, isHtml: true);
    }
}
