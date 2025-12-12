
using System;

namespace UserService.Infrastructure.CrossCutting;

public interface ICorrelationContext
{
    string CorrelationId { get; }

    void Set(string correlationId);
}
