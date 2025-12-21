
using System;

namespace UserService.Application.Services.EmailService;

public enum EmailTemplateType
{
   Welcome,
   ResetPassword,
}

public interface IEmailTemplateResolver
{
   Task<string> ResolveAsync(
      EmailTemplateType emailTemplateType,
      CancellationToken cancellationToken = default
   );
}