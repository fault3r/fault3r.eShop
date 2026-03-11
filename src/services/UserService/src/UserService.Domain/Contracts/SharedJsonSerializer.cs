
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Contracts;

public static class SharedJsonSerializer
{
    public static JsonSerializerOptions DefaultOptions => defaultOptions;

    private readonly static JsonSerializerOptions defaultOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    static SharedJsonSerializer()
    {
        defaultOptions.Converters.Add(new IdentityJsonConverter());
        defaultOptions.Converters.Add(new EmailJsonConverter());
        defaultOptions.Converters.Add(new FullNameJsonConverter());
    }
}

public class IdentityJsonConverter : JsonConverter<Identity>
{
    public override Identity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Identity.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, Identity value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Value.ToString());
}

public class EmailJsonConverter : JsonConverter<Email>
{
    public override Email? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Email.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Value);
}

public class FullNameJsonConverter : JsonConverter<FullName>
{
    public override FullName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => FullName.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, FullName value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}