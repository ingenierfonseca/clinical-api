using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/resource-type")]
    public class ResourceTypeController(IResourceTypeService resourceTypeService) : Controller
    {
        private readonly IResourceTypeService _resourceTypeService = resourceTypeService;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _resourceTypeService.GetAllAsync<ResourceTypeDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _resourceTypeService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ResourceTypeDto p)
        {
            var result = await _resourceTypeService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, ResourceTypeDto p)
        {
            var result = await _resourceTypeService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
