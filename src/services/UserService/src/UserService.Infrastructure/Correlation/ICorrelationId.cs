
using System;

namespace UserService.Infrastructure.Correlation;

public interface ICorrelationContext
{
    string CorrelationId { get; }

    void Set(string correlationId);
}
