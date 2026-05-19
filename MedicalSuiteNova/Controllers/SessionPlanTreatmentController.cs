using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/session-plan-treatment")]
    public class SessionPlanTreatmentController(ISessionPlanMasterService _sessionPlaService) : Controller
    {
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
            var item = await _sessionPlaService.FirstOrDefaultAsync<SessionPlanMasterDto>
            (
                s => s.Id == id, 
                q => q.Include(s => s.Items)!
                    .ThenInclude(i => i.TemplateItem)
            );
            return Ok(item);
        }

        [HttpGet("treatment-history/{id:int}")]
        public async Task<IActionResult> GetByCustomer(int id)
        {
            var item = await _sessionPlaService.GetByCustomer(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestSessionPlanMaster p)
        {
            var result = await _sessionPlaService.AddAsync(p);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result);
        }

        [HttpPost("change-status")]
        public async Task<IActionResult> ChangeStatus(RequestStatusSessionPlanMaster s)
        {
            var result = await _sessionPlaService.ChangeStatus(s);

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
