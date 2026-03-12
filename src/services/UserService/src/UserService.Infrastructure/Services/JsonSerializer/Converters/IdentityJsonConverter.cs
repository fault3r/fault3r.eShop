
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.CrossCutting.JsonConverters;

public sealed class IdentityJsonConverter : JsonConverter<Identity>
{
    public override Identity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return Identity.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, Identity value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value.ToString());
    }
}
