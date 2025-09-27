using System;
using System.Collections;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.DTOs
{
    public class RepositoryResult
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public IEnumerable<Item> Items { get; set; } = [];  
    }
}