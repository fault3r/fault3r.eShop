
using System;

namespace UserService.Infrastructure.Settings;

public class AppSettings
{
    public required MetadataSettings Metadata { get; set; }
    public required CommonSettings Common { get; set; }

    public class MetadataSettings
    {
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public required string ApiVersion { get; set; }
    }

    public class CommonSettings
    {
        public required string UrlVersionSegment { get; set; }
        public required string VersionHeader { get; set; }
        public required string CorrelationHeader { get; set; }
    }
}
