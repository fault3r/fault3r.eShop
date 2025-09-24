using System;
using CatalogManagementService.Domain.Interfaces;

namespace CatalogManagementService.Application.Services
{
    public class CatalogService(ICatalogRepository catalogRepository)
    {
        private readonly ICatalogRepository _catalogRepository = catalogRepository;


        

    }
}