
using System;

namespace UserService.Infrastructure.Settings;

public class AppSetting
{
    public required ServiceSetting Service { get; set; }

    public required string ApiVersion { get; set; }
    public required string UrlVersionSegment { get; set; }
    public required string VersionHeader { get; set; }
    public required string CorrelationHeader { get; set; }
    public required string ContentRoot { get; set; }

    public class ServiceSetting
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Version { get; set; }
    }
}
