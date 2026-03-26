
using System;
using UserService.Application.CrossCutting;
using UserService.Infrastructure.Exceptions.CrossCutting;

namespace UserService.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string> id = new();

    public string CorrelationId
        => id.Value
            ?? throw new CorrelationIdUnsetException();

    public void Set(string correlationId)
    {
        ArgumentException.ThrowIfNullOrEmpty(correlationId);

        id.Value = correlationId;
    }
}
