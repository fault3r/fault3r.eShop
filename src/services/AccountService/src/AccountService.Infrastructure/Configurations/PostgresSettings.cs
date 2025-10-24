
using System;

namespace AccountService.Infrastructure.Configurations
{
    public class PostgresSettings
    {
        public required string Host{ get; set; }

        public required int Port { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public required string Database { get; set; }
    }
}
