
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

        public Task<bool> LogInformation(string message)
        {
            try
            {
                var log = $"⋄[{typeof(TLog).Name}] {message} ⟶{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                _logger.Information(log);
                return Task.FromResult(true);
            }
            catch{ throw; }
        }
    }
}
