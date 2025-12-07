
using System;

namespace UserService.Infrastructure.Logging;

public interface ILogger
{
    void LogInformation(string log);
    void LogWarning(string log);
    void LogError(string log);

    void LogError(Exception exception, string log);

    void LogInformation(string log, IDictionary<string, object> properties);
     void LogWarning(string log, IDictionary<string, object> properties);
    void LogError(string log, IDictionary<string, object> properties);

    void LogError(Exception exception, string log, IDictionary<string, object> properties);
}
