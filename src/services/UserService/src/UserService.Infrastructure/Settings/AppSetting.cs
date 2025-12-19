
using System;

namespace UserService.Infrastructure.Settings;

public class AppSetting
{
    public required MetadataSetting Metadata { get; set; }
    public required CommonSetting Common { get; set; }

    public class MetadataSetting
    {
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public required string ApiVersion { get; set; }
    }

    public class CommonSetting
    {
        public required string UrlVersionSegment { get; set; }
        public required string VersionHeader { get; set; }
        public required string CorrelationHeader { get; set; }
    }
}
