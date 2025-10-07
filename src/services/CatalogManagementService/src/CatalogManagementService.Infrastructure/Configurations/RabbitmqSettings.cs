using System;

namespace CatalogManagementService.Infrastructure.Configurations
{
    public class RabbitmqSettings
    {
        public required string HostName { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public required string ExchangeName { get; set; }

        public required string QueueName { get; set; }
    }
}