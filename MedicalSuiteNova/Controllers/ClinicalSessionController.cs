using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalSession;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/clinical-session")]
    public class ClinicalSessionController(IClinicalSessionService clinicalSessionService) : Controller
    {
        private readonly IClinicalSessionService _clinicalSessionService = clinicalSessionService;

        [HttpGet]
        public async Task<ActionResult<PagedResponse<ClinicalSessionDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _clinicalSessionService.GetAllAsync<ClinicalSessionDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("active")]
        public async Task<ActionResult<PagedResponse<ClinicalSessionDto>>> GetActive(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _clinicalSessionService.GetAllAsync<ClinicalSessionDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("customer/{id:int}/short-info")]
        public async Task<ActionResult<IEnumerable<ClinicalSessionShortInfoDto>>> GetShortInfoByCustomer(int id)
        {
            var items = await _clinicalSessionService.GetShortInfoByCustomer(id);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _clinicalSessionService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ClinicalSessionDto p)
        {
            var result = await _clinicalSessionService.AddAsync(p);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateTreatmentDto p)
        {
            var result = await _clinicalSessionService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
