using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentTypeController : Controller
    {
        private readonly IAppointmentTypeService _appointmentTypeService;

        public AppointmentTypeController(IAppointmentTypeService appointmentTypeService)
        {
            _appointmentTypeService = appointmentTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<AppointmentType>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var appointments = await _appointmentTypeService.GetAllAsync(pageNumber, pageSize);
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _appointmentTypeService.FyndAsync(id);
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AppointmentType p)
        {
            var result = await _appointmentTypeService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(AppointmentType p)
        {
            var result = await _appointmentTypeService.UpdateAsync(p);
            return Ok(result);
        }
    }
}
