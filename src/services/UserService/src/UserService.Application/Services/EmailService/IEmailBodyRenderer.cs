
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailBodyRenderer
{
    Task<string> RenderAsync(
        string template,
        object model,
        CancellationToken cancellationToken = default
    );
}
