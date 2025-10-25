
using System;

namespace AccountService.Application.Interfaces.Services
{
    public interface ILoggerService<>
    {
        Task LogInformation(string message);

        Task LogError(string message);
    }
}
