
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.CrossCutting.JsonSerializer;

public sealed class ValueObjectJsonConverter<TValueObject> : JsonConverter<TValueObject>
    where TValueObject : IValueObject
{
    public override TValueObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = reader.GetString();

        if (string.IsNullOrEmpty(json))
            return default;

        var parser = typeToConvert.GetMethod("Parse", [typeof(string)])
            ?? throw new JsonException($"parse method not found for type '{typeToConvert.Name}'");
            
        return (TValueObject?)parser.Invoke(default, [reader.GetString()]);
    }

    public override void Write(Utf8JsonWriter writer, TValueObject value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.ToString());
}
