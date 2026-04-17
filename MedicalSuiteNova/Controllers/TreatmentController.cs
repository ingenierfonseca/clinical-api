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
    [Route("api/[controller]")]
    public class TreatmentController : Controller
    {
        private readonly ITreatmentService _treatmentService;

        public TreatmentController(ITreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<TreatmentDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var appointments = await _treatmentService.GetAllAsync<TreatmentDto>(pageNumber, pageSize, new Expression<Func<Treatment, object>>[] { x => x.Currency });
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _treatmentService.FindAsync(id);
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TreatmentDto p)
        {
            var result = await _treatmentService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateTreatmentDto p)
        {
            var result = await _treatmentService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
