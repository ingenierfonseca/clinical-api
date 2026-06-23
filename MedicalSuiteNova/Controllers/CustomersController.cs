using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Customer;
using MedicalSuiteNova.Domain.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var items = await _customerService.GetAllAsync<CustomerDto>(
                pageNumber, 
                pageSize,
                a => search != string.Empty && a.FirstName.Contains(search) || a.LastName.Contains(search),
                query => query.OrderBy(a => a.FirstName)
            );
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _customerService.FindAsync(id);
            return Ok(item);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var items = await _customerService.GetDashboard();
            return Ok(items);
        }

        [HttpGet("{id:int}/risk-dashboard")]
        public async Task<IActionResult> GetCustomerRiskDashboard(int id)
        {
            var items = await _customerService.GetCustomerRiskDashboard(id);
            return Ok(items);
        }

        [HttpGet("{id:int}/next-appointment")]
        public async Task<IActionResult> GetCustomerNextAppointment(int id)
        {
            var items = await _customerService.GetCustomerNextAppointment(id);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateCustomerDto dto)
        {
            var result = await _customerService.AddAsync(dto);
            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPost("bulk-import")]
        public async Task<IActionResult> BulkImport([FromBody] List<CustomerImportDto> dtos)
        {
            if (dtos == null || dtos.Count == 0) return BadRequest("No hay datos para importar.");

            var result = await _customerService.BulkImport(dtos);
            return Ok(result);
        }

        [HttpPost("{id}/upload-avatar")]
        public async Task<IActionResult> UploadAvatar(int id, IFormFile file)
        {
            var result = await _customerService.UploadAvatarAsync(id, file);
            return result.IsSuccess ? Ok(result) : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateCustomerDto dto)
        {
            var result = await _customerService.UpdateAsync(id, dto);
            return result.IsSuccess
                ? Ok(result.Value) 
                : BadRequest(new { message = result.ErrorMessage });
        }
    }
}
