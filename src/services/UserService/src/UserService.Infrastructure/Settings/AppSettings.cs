
using System;

namespace UserService.Infrastructure.Settings;

public sealed class AppSettings
{
    public required AppServiceSettings Service { get; init; }

    public required AppUrlSettings Urls{ get; init; }
    public required string ApiVersion { get; init; }
    public required string UrlVersionSegment { get; init; }
    public required string VersionHeader { get; init; }
    public required string CorrelationHeader { get; init; }
    public required string ContentRoot { get; init; }
    public required string SessionLifetimeDays { get; init; }

    public sealed class AppServiceSettings
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required string Version { get; init; }
    }

    public sealed class AppUrlSettings
    {
        public required string Http { get; init; }
        public required string Https { get; init; }
    }
}
