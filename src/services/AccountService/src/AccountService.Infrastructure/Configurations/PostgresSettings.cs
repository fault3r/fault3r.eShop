
using System;

namespace AccountService.Infrastructure.Configurations
{
    public class PostgresSettings
    {
        public required string ConnectionString { get; set; }

        public required string DatabaseName { get; set; }
        
        public required string TableName { get; set; }
    }
}
