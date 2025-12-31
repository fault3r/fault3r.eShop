
using System;
using UserService.Application.Services.EmailService;
using UserService.Infrastructure.Exceptions.Services.EmailService;

namespace UserService.Infrastructure.Services.EmailService;

public sealed class EmailTemplateRenderer : IEmailTemplateRenderer
{
    public Task<string> RenderAsync<T>(
        string template,
        T model,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(template);
        ArgumentNullException.ThrowIfNull(model);

        string rendered = template;

        var props = typeof(T)
            .GetProperties()
            .Where(p => p.CanRead);

        foreach (var prop in props)
        {
            string token = "{{" + prop.Name + "}}";
            var value = prop.GetValue(model)
                ?? throw new CannotRenderEmailTemplateException();
            
            rendered = rendered.Replace(token, value as string);
        }

        return Task.FromResult(rendered);
    }
}