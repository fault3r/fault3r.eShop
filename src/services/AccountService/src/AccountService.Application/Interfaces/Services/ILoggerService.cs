
using System;

namespace AccountService.Application.Interfaces.Services
{
    public interface ILoggerService<in TLog>
    {
        Task LogInformation(string message);

        Task LogError(string message);
    }
}
