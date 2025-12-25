
using System;
using UserService.Infrastructure.Exceptions.CrossCutting;

namespace UserService.Infrastructure.CrossCutting;

public sealed class CorrelationContext : ICorrelationContext
{
    private static readonly AsyncLocal<string> asyncLocalId
        = new();

    public string CorrelationId { get; }
        = asyncLocalId.Value ?? string.Empty;

    public void Set(string correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
            throw new MissingCorrelationIdException();

        asyncLocalId.Value = correlationId;
    }
}
