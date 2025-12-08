
using System;

namespace UserService.Infrastructure.Settings;

public class PostgresSettings
{
    public required string Host { get; set; } 
    public required int Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Database { get; set; } 

    public string ToConnectionString()
    {
        return $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";
    }
}
