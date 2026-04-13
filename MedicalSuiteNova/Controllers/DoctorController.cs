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
    public class DoctorController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<Doctor>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var appointments = await _doctorService.GetAllAsync(pageNumber, pageSize);
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var appointment = await _doctorService.FyndAsync(id);
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Doctor p)
        {
            var result = await _doctorService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Doctor p)
        {
            var result = await _doctorService.UpdateAsync(p);
            return Ok(result);
        }
    }
}
