using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var items = await _customerService.GetAllPaginatedAsync(pageNumber, pageSize, search);
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

        [HttpPost]
        public async Task<IActionResult> Post(Customer p)
        {
            p.CreatedAt = DateTime.Now;
            await _customerService.AddAsync(p);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, Customer p)
        {
            //TODO
            var customer = await _customerService.FindAsync(id);
            if (customer == null)
                return NotFound();

            customer.FirstName = p.FirstName;
            customer.LastName = p.LastName;
            customer.Age = p.Age;
            customer.Email = p.Email;
            customer.Phone = p.Phone;
            customer.Avatar = p.Avatar;

            await _customerService.UpdateAsync(id, customer);
            return Ok();
        }
    }
}
