
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserService.Application.CrossCutting;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.CrossCutting.JsonSerializer;

public sealed class NetJsonSerializer : IJsonSerializer
{
    public JsonSerializerOptions Options => options;

    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    public NetJsonSerializer()
    {
        foreach (var conv in converters)
            options.Converters.Add(conv);
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