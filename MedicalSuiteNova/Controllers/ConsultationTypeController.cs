using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/consultation-type")]
    public class ConsultationTypeController(IConsultationTypeService service) : Controller
    {
        private readonly IConsultationTypeService _service = service;

        [HttpGet]
        [Authorize(Policy = AppPolicies.CanViewConsultationTypes)]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _service.GetAllAsync<ConsultationTypeDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("active")]
        [Authorize(Policy = AppPolicies.CanViewConsultationTypes)]
        public async Task<IActionResult> GetActive(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _service.GetAllAsync<ConsultationTypeDto>(
                pageNumber, pageSize,
                x => x.IsActive == true, null);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPolicies.CanViewConsultationTypes)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = AppPolicies.CanCreateConsultationTypes)]
        public async Task<IActionResult> Post([FromBody] ConsultationTypeDto p)
        {
            var result = await _service.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPolicies.CanEditConsultationTypes)]
        public async Task<IActionResult> Put(int id, [FromBody] ConsultationTypeDto p)
        {
            var result = await _service.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
