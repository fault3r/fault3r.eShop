
using System;
using System.Text.Json;

namespace UserService.Application.CrossCutting;

public interface IJsonSerializer
{
    JsonSerializerOptions DefaultOptions { get; }
}
