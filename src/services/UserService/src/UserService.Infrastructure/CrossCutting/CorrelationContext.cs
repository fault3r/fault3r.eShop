
using System;
using UserService.Infrastructure.Exceptions.Correlation;

namespace UserService.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string> _id = new();

    public string CorrelationId
    {
        get => _id.Value
            ?? throw new MissingCorrelationIdException();
    }

    public void Set(string correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
            throw new MissingCorrelationIdException();

        _id.Value = correlationId;
    }
}
