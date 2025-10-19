
using System;

namespace CatalogManagementService.Infrastructure.Exceptions
{
    public class LoggerInternalException : Exception
    {
        public LoggerInternalException(){}

        public LoggerInternalException(string message) 
            : base(message) { }

        public LoggerInternalException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
