
using System;

namespace UserService.Infrastructure.Exceptions.Services.EmailService;

public sealed class CannotSendEmailException(
    string value
) : InfrastructureException($"cannot send email to: {value}") { }