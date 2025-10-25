
using System;

namespace AccountService.Api.Exceptions
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException()
            : base($"unable to set up the service!") { }
    }
}
