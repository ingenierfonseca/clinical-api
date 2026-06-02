using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TreatmentController(ITreatmentService treatmentService) : Controller
    {
        private readonly ITreatmentService _treatmentService = treatmentService;

        [HttpGet]
        [Authorize(Policy = AppPolicies.CanViewTreatments)]
        public async Task<ActionResult<PagedResponse<TreatmentDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _treatmentService.GetAllAsync<TreatmentDto>(pageNumber, pageSize, null, null,  x => x.Currency!);
            return Ok(items);
        }

        [HttpGet("active")]
        [Authorize(Policy = AppPolicies.CanViewTreatments)]
        public async Task<ActionResult<PagedResponse<TreatmentDto>>> GetActive(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _treatmentService.GetAllAsync<TreatmentDto>(
                pageNumber,
                pageSize,
                x => x.IsActive == true, null);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPolicies.CanViewTreatments)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _treatmentService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(Policy = AppPolicies.CanCreateTreatments)]
        public async Task<IActionResult> Post(TreatmentDto p)
        {
            var result = await _treatmentService.CreateAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPolicies.CanEditTreatments)]
        public async Task<IActionResult> Put(int id, UpdateTreatmentDto p)
        {
            var result = await _treatmentService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
