
using System;
using System.Text.Json;
using UserService.Application.CrossCutting;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.CrossCutting.JsonSerializer;

public sealed class NetJsonSerializer : IJsonSerializer
{
    public JsonSerializerOptions DefaultOptions => _options;

    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
        Converters = {
            new ValueObjectJsonConverter<Identity>(),
            new ValueObjectJsonConverter<Email>(),
            new ValueObjectJsonConverter<FullName>(),
        },
    };
}
