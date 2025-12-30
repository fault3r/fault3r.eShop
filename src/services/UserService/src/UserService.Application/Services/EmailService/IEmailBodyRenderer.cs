
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailBodyRenderer
{
    Task<string> RenderAsync<T>(
        string template,
        T model,
        CancellationToken cancellationToken = default
    );
}
