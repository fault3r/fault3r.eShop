
using System;

namespace CatalogManagementService.Infrastructure.Configurations
{
    public class ContextSettings
    {
        public required string ConnectionString { get; set; }

        public required string DatabaseName { get; set; }
        
        public required string CollectionName { get; set; }
    }
}