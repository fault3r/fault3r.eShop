
using System;

namespace UserService.Infrastructure.Settings;

public class FluentEmailSetting
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string TemplatesPath { get; set; }
}