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
    [Route("api/session-plan-treatment")]
    public class SessionPlanTreatmentController : Controller
    {
        private readonly ISessionPlanMasterService _sessionPlaService;

        public SessionPlanTreatmentController(ISessionPlanMasterService sessionPlaService)
        {
            _sessionPlaService = sessionPlaService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<SessionPlanMasterDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _sessionPlaService.GetAllAsync<SessionPlanMasterDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("active")]
        public async Task<ActionResult<PagedResponse<SessionPlanMasterDto>>> GetActive(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _sessionPlaService.GetAllAsync<SessionPlanMasterDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _sessionPlaService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(SessionPlanMasterDto p)
        {
            var result = await _sessionPlaService.AddAsync(p);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateTreatmentDto p)
        {
            var result = await _sessionPlaService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
