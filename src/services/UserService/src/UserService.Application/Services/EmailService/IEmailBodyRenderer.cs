
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailBodyRenderer
{
    Task<string> RenderRazorBody(
        string templatePath,
        object model,
        CancellationToken cancellationToken = default
    );
}
