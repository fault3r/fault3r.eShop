
using System;
using System.Text.Json;

namespace UserService.Application.Services.JsonSerializer;

public interface IJsonSerializer
{
    JsonSerializerOptions DefaultOptions { get; }
}
