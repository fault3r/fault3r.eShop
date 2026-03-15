
using System;
using System.Text.Json;
using UserService.Application.CrossCutting;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.CrossCutting.JsonSerializer;

public sealed class NetJsonSerializer : IJsonSerializer
{
    public JsonSerializerOptions Options => options;

    private static readonly JsonSerializerOptions options = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters =
        {
            new ValueObjectJsonConverter<Identity>(),
            new ValueObjectJsonConverter<Email>(),
            new ValueObjectJsonConverter<PasswordHash>(),
            new ValueObjectJsonConverter<PasswordSalt>(),
            new ValueObjectJsonConverter<FullName>(),
            new ValueObjectJsonConverter<Role>(),
            new ValueObjectJsonConverter<Status>()
        },
    };

    public string Serialize(object value)
        => System.Text.Json.JsonSerializer.Serialize(value, options);

    public string Serialize(object value, Type inputType)
        => System.Text.Json.JsonSerializer.Serialize(value, inputType, options);

    public object? Deserialize(string json, Type returnType)
        => System.Text.Json.JsonSerializer.Deserialize(json, returnType, options);

    public TReturnType? Deserialize<TReturnType>(string json)
        => System.Text.Json.JsonSerializer.Deserialize<TReturnType>(json, options);
}