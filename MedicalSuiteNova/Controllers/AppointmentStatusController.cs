using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/appointment-status")]
    public class AppointmentStatusController(IAppointmentStatusService appointmentStatusService) : Controller
    {
        private readonly IAppointmentStatusService _appointmentStatusService = appointmentStatusService;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _appointmentStatusService.GetAllAsync<AppointmentStatusDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _appointmentStatusService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(AppointmentStatusDto p)
        {
            var result = await _appointmentStatusService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, AppointmentStatusDto p)
        {
            var result = await _appointmentStatusService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
