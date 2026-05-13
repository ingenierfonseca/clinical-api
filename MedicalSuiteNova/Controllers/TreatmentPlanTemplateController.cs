using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/treatment-plan-template")]
    public class TreatmentPlanTemplateController : Controller
    {
        private readonly ITreatmentPlanTemplateService _treatmentPlanService;

        public TreatmentPlanTemplateController(ITreatmentPlanTemplateService treatmentPlanService)
        {
            _treatmentPlanService = treatmentPlanService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TreatmentPlanTemplateDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _treatmentPlanService.GetAllAsync<TreatmentPlanTemplateDto>(pageNumber, pageSize, null, null, new Expression<Func<TreatmentPlanTemplate, object>>[] { x => x.Items! });
            return Ok(items);
        }

        [HttpGet("active")]
        public async Task<ActionResult<PagedResponse<TreatmentPlanTemplateDto>>> GetActive(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _treatmentPlanService.GetAllAsync<TreatmentPlanTemplateDto>(
                pageNumber,
                pageSize,
                x => x.IsActive == true, null, x => x.Items!, x => x.Currency!);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _treatmentPlanService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TreatmentPlanTemplateDto p)
        {
            var result = await _treatmentPlanService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateTreatmentDto p)
        {
            var result = await _treatmentPlanService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
