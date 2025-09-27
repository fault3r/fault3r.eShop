using CatalogManagementService.Application.DTOs;
using CatalogManagementService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogManagementService.Api.Controllers
{
    [Route("api/v1/catalog")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CatalogController(ICatalogService catalogService) : ControllerBase
    {
        private readonly ICatalogService _catalogService = catalogService;

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var items = await _catalogService.GetAllAsync();
            return Ok(items);
        }
    }
}
