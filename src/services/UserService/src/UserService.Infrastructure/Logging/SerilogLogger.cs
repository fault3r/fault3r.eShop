using System;
using System.Collections.Generic;
using Serilog;

namespace UserService.Infrastructure.Logging;

public sealed class SerilogLogger(Serilog.ILogger logger) : ILogger
{
    private readonly Serilog.ILogger _logger = logger;

    public void LogInformation(string log) => _logger.Information(log);

    public void LogWarning(string log) => _logger.Warning(log);

    public void LogError(string log) => _logger.Error(log);

    public void LogError(Exception exception, string log)
        => _logger.Error(exception, log);

    public void LogInformation(string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Information(log);

    public void LogWarning(string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Warning(log);

    public void LogError(string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Error(log);

    public void LogError(Exception exception, string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Error(exception, log);

    private Serilog.ILogger ContextualLogger(IDictionary<string, object>? properties)
    {
        var contextualLogger = _logger;
        if (properties != null)
            foreach (var kvp in properties)
            {
                contextualLogger = contextualLogger
                    .ForContext(kvp.Key, kvp.Value, destructureObjects: true);
            }
        return contextualLogger;
    }
}
