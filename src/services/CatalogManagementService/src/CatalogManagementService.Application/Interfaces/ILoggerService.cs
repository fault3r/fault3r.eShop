
using System;

namespace CatalogManagementService.Application.Interfaces
{
    public interface ILoggerService<in TLog>
    {
        Task<bool> LogInformation(string message);
    }
}
