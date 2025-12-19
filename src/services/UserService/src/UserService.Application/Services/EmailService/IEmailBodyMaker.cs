
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailBodyMaker
{
    string ResolveTemplatePath(
        EmailTemplateType templateType
    );
    
    Task<string> RenderRazorBody(
        string templatePath,
        object model,
        CancellationToken cancellationToken = default
    );
}
