using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Doctor;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController(IDoctorService doctorService) : Controller
    {
        private readonly IDoctorService _doctorService = doctorService;

        [HttpGet]
        public async Task<ActionResult<PagedResponse<DoctorInfoDto>>> Get(
            [FromQuery] int? specialtyId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var appointments = await _doctorService.GetAllAsync(
                pageNumber,
                pageSize,
                specialtyId
            );
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var doctor = await _doctorService.FindAsync(id);
            if (doctor == null)
                return BadRequest(new { message = "Id no encontrado" });
            return Ok(doctor);
        }

        [HttpGet("{id:int}/info")]
        public async Task<IActionResult> GetInfo(int id)
        {
            var result = await _doctorService.GetInfo(id);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDoctorDto p)
        {
            var result = await _doctorService.AddAsync(p);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateDoctorDto p)
        {
            var result = await _doctorService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
