
using System;

namespace UserService.Infrastructure.Logging;

public interface ILogger
{
    void Information(string log);
    void Warning(string log);
    void Error(string log);

    void Error(Exception exception, string log);

    void Information(string log, IDictionary<string, object> properties);
     void Warning(string log, IDictionary<string, object> properties);
    void Error(string log, IDictionary<string, object> properties);

    void Error(Exception exception, string log, IDictionary<string, object> properties);
}
