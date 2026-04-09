using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _patientRepository;

        public CustomersController(ICustomerRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var patients = await _patientRepository.GetAllPaginatedAsync(pageNumber, pageSize, search);
            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var patients = await _patientRepository.GetByIdAsync(id);
            return Ok(patients);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var patients = await _patientRepository.GetDashboard();
            return Ok(patients);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer p)
        {
            p.CreatedAt = DateTime.Now;
            await _patientRepository.AddAsync(p);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Customer p)
        {
            var customer = await _patientRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            customer.FirstName = p.FirstName;
            customer.LastName = p.LastName;
            customer.Age = p.Age;
            customer.Email = p.Email;
            customer.Phone = p.Phone;
            customer.Avatar = p.Avatar;

            await _patientRepository.UpdateAsync(customer);
            return Ok();
        }
    }
}
