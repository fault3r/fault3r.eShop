
using System;
using System.Text.Json;
using UserService.Application.Services.JsonSerializer;
using UserService.Infrastructure.Services.JsonSerializer.Converters;

namespace UserService.Infrastructure.Services.JsonSerializer;

public sealed class NetJsonSerializer : IJsonSerializer
{
    public JsonSerializerOptions DefaultOptions => _options;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        Converters = { new IdentityJsonConverter() },
    };
}
