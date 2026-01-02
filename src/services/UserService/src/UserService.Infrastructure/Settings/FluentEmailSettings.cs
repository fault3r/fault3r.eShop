
using System;

namespace UserService.Infrastructure.Settings;

public sealed class FluentEmailSettings
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string TemplatesPath { get; init; }
}