
using System;

namespace CatalogManagementService.Api.Exceptions
{
    public class SettingsReferenceException : Exception
    {
        public SettingsReferenceException() { }

        public SettingsReferenceException(string message) : base(message) { }
    }
}
