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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<AppointmentInfoDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var appointments = await _appointmentService.GetAllAsync<AppointmentInfoDto>(pageNumber, pageSize);
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _appointmentService.FindAsync(id);
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AppointmentDto p)
        {
            var result = await _appointmentService.AddAsync(p);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, AppointmentDto dto)
        {
            var result = await _appointmentService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);
            return Ok(result.Value);
        }
    }
}
