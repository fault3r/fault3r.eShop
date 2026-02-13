
using System;
using Microsoft.AspNetCore.Http;

namespace UserService.Infrastructure.DependencyInjection;

public static class HttpContextExtensions
{
    public static string? UserId(this HttpContext context)
        => context.User.FindFirst("sub")?.Value;

    public static string? SessionId(this HttpContext context)
        => context.User.FindFirst("jti")?.Value;
}
