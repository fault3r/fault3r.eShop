
using System;

namespace UserService.Infrastructure.Settings;

public sealed class AppSettings
{
    public required AppServiceSettings Service { get; init; }

    public required AppUrlSettings Urls { get; init; }
    
    public required float ApiVersion { get; init; }
    public required string VersionParameter { get; init; }
    public required string VersionHeader { get; init; }
    public required string CorrelationHeader { get; init; }

    public sealed class AppServiceSettings
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required float Version { get; init; }
    }

    public sealed class AppUrlSettings
    {
        public required string Http { get; init; }
        public required string Https { get; init; }
    }
}
