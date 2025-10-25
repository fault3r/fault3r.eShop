
using System;

namespace AccountService.Application.Interfaces.Services
{
    public interface ILoggerService<in TContext>
    {
        Task LogInformation(string message);

        Task LogError(string message);
    }
}
