
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailTemplateResolver
{
   Task<string> ResolveAsync(
      EmailTemplateType emailTemplateType,
      CancellationToken cancellationToken = default
   );
}