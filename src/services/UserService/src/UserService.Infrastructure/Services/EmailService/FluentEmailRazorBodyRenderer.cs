
using System;
using FluentEmail.Core;
using FluentEmail.Razor;
using UserService.Application.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class FluentEmailRazorBodyRenderer(RazorRenderer razorRenderer) : IEmailBodyRenderer
{
    private readonly RazorRenderer _renderer = razorRenderer;

    public async Task<string> RenderAsync<T>(
        string template,
        T model,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(template);
        ArgumentNullException.ThrowIfNull(model);

        var rendered = await _renderer.ParseAsync(template, model, isHtml: true);

        return rendered;
    }
}
