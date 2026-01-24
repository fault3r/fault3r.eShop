
using System;

namespace UserService.Infrastructure.Settings;

public sealed class SessionSettings
{
    public required string SessionKey { get; init; }
    public required string UserSessionsKey { get; init; }

    public required int MaxSessionsPerUser { get; init; }
}
