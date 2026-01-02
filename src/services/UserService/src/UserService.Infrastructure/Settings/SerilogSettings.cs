
using System;

namespace UserService.Infrastructure.Settings;

public sealed class SerilogSettings
{
    public required string Filename { get; init; }
}
