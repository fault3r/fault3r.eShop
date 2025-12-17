
using System;
using UserService.Infrastructure.Exceptions.CrossCutting;

namespace UserService.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string> _correlationId
        = new();

    public string CorrelationId
    {
        get => _correlationId.Value
            ?? throw new MissingCorrelationIdException();
    }

    public void Set(string correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
            throw new MissingCorrelationIdException();

        _correlationId.Value = correlationId;
    }
}
