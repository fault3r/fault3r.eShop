
using System;
using UserService.Application.CrossCutting;
using UserService.Infrastructure.Exceptions.CrossCutting;

namespace UserService.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string> asyncLocalId = new();

    public string CorrelationId
        => asyncLocalId.Value
            ?? throw new CorrelationIdUnsetException();

    public void Set(string correlationId)
    {
        ArgumentException.ThrowIfNullOrEmpty(correlationId);

        asyncLocalId.Value = correlationId;
    }
}
