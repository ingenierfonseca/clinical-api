using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalNotes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicalNotesController(IClinicalNotesService clinicalNotesService) : ControllerBase
    {
        private readonly IClinicalNotesService _clinicalNotesService = clinicalNotesService;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var items = await _clinicalNotesService.GetAllAsync<ClinicalNotesDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var note = await _clinicalNotesService.FindAsync(id);
            if (note == null)
                return NotFound(new { status = 404, errors = new[] { "Nota clínica no encontrada" } });
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClinicalNotesDto dto)
        {
            var result = await _clinicalNotesService.AddAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClinicalNotesDto dto)
        {
            var result = await _clinicalNotesService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }
    }
}
