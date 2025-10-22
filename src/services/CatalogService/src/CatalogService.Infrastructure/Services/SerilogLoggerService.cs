
using System;
using Serilog;
using CatalogService.Application.Interfaces;

namespace CatalogService.Infrastructure.Services
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

        public Task LogInformation(string message)
        {
            _logger.Information(ToLogString(message));
            return Task.CompletedTask;
        }

        public Task LogError(string message)
        {
            _logger.Error(ToLogString(message));
            return Task.CompletedTask;
        }
    }
}