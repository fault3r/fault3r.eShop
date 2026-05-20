
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailTemplateRenderer
{
    string RenderAsync<T>(
        string template,
        T model,
        CancellationToken cancellationToken
    );
}
