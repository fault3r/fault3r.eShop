
using System;
using System.Text.Json;
using UserService.Application.CrossCutting;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.CrossCutting.JsonSerializer;

public sealed class AppJsonSerializer : IJsonSerializer
{
    public JsonSerializerOptions DefaultOptions => options;

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
}