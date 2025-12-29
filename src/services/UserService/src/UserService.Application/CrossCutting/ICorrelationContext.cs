
using System;

namespace UserService.Application.CrossCutting;

public interface ICorrelationContext
{
    string CorrelationId { get; }

    void Set(string correlationId);
}
