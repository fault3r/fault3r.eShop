
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
        var mapper = typeToConvert.GetMethod("Parse", [typeof(string)])!;
        return (TValueObject?)mapper.Invoke(null, [reader.GetString()]);
    }

    public override void Write(Utf8JsonWriter writer, TValueObject value, JsonSerializerOptions options)
        => writer.WriteStringValue(value?.ToString());
}
