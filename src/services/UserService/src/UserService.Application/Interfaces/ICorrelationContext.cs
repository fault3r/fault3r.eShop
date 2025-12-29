
using System;

namespace UserService.Application.Interfaces;

public interface ICorrelationContext
{
    string CorrelationId { get; }

    void Set(string correlationId);
}
