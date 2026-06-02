
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController(IStaffService staffService) : Controller
    {
        private readonly IStaffService _staffService = staffService;

        [HttpGet]
        public async Task<ActionResult<PagedResponse<StaffDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _staffService.GetAllAsync<StaffDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _staffService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(StaffDto p)
        {
            var result = await _staffService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, StaffDto p)
        {
            var result = await _staffService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
