
using System;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ILoggerService<in TLog>
    {
        Task LogInformation(string message);

        Task LogError(string message);
    }
}
