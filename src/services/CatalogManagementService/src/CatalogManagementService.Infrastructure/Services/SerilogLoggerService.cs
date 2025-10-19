
using System;
using Serilog;
using CatalogManagementService.Application.Interfaces;

namespace CatalogManagementService.Infrastructure.Services
{
    public class SerilogLoggerService<TLog> : ILoggerService<TLog>
    {
        private readonly Serilog.ILogger _logger;

        public SerilogLoggerService()
        {
            _logger = Log.ForContext<TLog>();
        }

        private static string ToLogString(string message) =>
         $"⋄[{typeof(TLog).Name}] {message} ⟶{DateTime.Now:yyyy-MM-dd HH:mm:ss}";

        public Task<bool> LogInformation(string message)
        {
            try
            {
                _logger.Information(ToLogString(message));
                return Task.FromResult(true);
            }
            catch { throw; }
        }

        public Task<bool> LogError(string message)
        {            
            try
            {
                _logger.Error(ToLogString(message));
                return Task.FromResult(true);
            }
            catch { throw; }
        }
    }
}
