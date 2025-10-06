using System;

namespace CatalogManagementService.Infrastructure.Configurations
{
    public class RabbitmqSettings
    {
        public required string HostName { get; set; }

        public required string QueueName { get; set; }
    }
}