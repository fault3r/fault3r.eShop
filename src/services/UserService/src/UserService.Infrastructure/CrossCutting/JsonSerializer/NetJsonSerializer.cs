
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserService.Application.CrossCutting;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.CrossCutting.JsonSerializer;

public sealed class NetJsonSerializer : IJsonSerializer
{
    public static JsonSerializerOptions DefaultOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    public JsonSerializerOptions Options { get; }

    public NetJsonSerializer()
    {
        Options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

        foreach (var converter in converters)
            Options.Converters.Add(converter);
    }

    private readonly IEnumerable<JsonConverter> converters = [
        new ValueObjectJsonConverter<Identity>(),
        new ValueObjectJsonConverter<Email>(),
        new ValueObjectJsonConverter<PasswordHash>(),
        new ValueObjectJsonConverter<PasswordSalt>(),
        new ValueObjectJsonConverter<FullName>(),
        new ValueObjectJsonConverter<Role>(),
        new ValueObjectJsonConverter<Status>()
    ];
}