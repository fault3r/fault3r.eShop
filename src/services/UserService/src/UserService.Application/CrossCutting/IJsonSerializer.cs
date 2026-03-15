
using System;
using System.Text.Json;

namespace UserService.Application.CrossCutting;

public interface IJsonSerializer
{
    string Serialize(object value);
    string Serialize(object value, Type inputType);

    object? Deserialize(string json, Type returnType);
    TReturnType? Deserialize<TReturnType>(string json);
    
    JsonSerializerOptions Options { get; }
}
