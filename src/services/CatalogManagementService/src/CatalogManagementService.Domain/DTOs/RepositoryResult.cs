
using System;
using CatalogManagementService.Domain.Entities;

namespace CatalogManagementService.Domain.DTOs
{
    public class RepositoryResult
    {
        public int Code { get; set; } = 0;

        public IEnumerable<Item> Items { get; set; } = [];
    }

    public enum RepositoryResultCode
    {
        Ok = 200,
        Created = 201,
        NoContent = 204,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500,
    }
}