
using System;

namespace CatalogManagementService.Infrastructure.Configurations
{
    public class ApplicationSettings
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public required decimal Version { get; set; }

        public required string Url { get; set; }
    }
}