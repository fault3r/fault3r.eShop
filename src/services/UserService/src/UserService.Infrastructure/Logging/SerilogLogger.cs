
using System;
using Serilog.Context;

namespace UserService.Infrastructure.Logging;

public sealed class Serilogger(Serilog.ILogger logger) : ILogger
{
    private readonly Serilog.ILogger _logger = logger;

    public void Information(string log) => _logger.Information(log);

    public void Warning(string log) => _logger.Warning(log);

    public void Error(string log) => _logger.Error(log);

    public void Error(Exception exception, string log)
        => _logger.Error(exception, log);

    public void Information(string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Information(log);

    public void Warning(string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Warning(log);

    public void Error(string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Error(log);

    public void Error(Exception exception, string log, IDictionary<string, object> properties)
        => ContextualLogger(properties).Error(exception, log);

    private Serilog.ILogger ContextualLogger(IDictionary<string, object> properties)
    {
        if (properties is null)
            return _logger;

        var contextualger = _logger;
        foreach (var kvp in properties)
        {
            contextualger = contextualger
                .ForContext(kvp.Key, kvp.Value, destructureObjects: true);
        }
        return contextualger;
    }
}
