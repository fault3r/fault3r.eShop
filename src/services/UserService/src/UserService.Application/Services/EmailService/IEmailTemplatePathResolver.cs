
using System;

namespace UserService.Application.Services.EmailService;

public interface IEmailTemplatePathResolver
{
   Task<string> ResolveAsync(
      EmailTemplateType emailTemplateType
   );
}