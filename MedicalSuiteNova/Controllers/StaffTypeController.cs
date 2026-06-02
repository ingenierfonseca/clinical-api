
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StaffTypeController(IStaffTypeService staffTypeService) : Controller
    {
        private readonly IStaffTypeService _staffTypeService = staffTypeService;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _staffTypeService.GetAllAsync<StaffTypeDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _staffTypeService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(StaffTypeDto p)
        {
            var result = await _staffTypeService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, StaffTypeDto p)
        {
            var result = await _staffTypeService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
