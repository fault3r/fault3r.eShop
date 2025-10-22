
using System;

namespace CatalogService.Application.Interfaces
{
    public interface ILoggerService<in TLog>
    {
        Task LogInformation(string message);

        Task LogError(string message);
    }
}
