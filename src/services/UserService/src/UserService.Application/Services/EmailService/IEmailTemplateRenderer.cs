
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailTemplateRenderer
{
    Task<string> RenderAsync<T>(
        string template,
        T model,
        CancellationToken ct = default
    );
}
