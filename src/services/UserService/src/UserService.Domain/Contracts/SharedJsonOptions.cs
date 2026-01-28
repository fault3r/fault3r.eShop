
using System;
using System.Text.Json;

namespace UserService.Domain.Contracts;

public static class SharedJsonOptions
{
    public static JsonSerializerOptions DefaultOptions => defaultOptions;

    private readonly static JsonSerializerOptions defaultOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
