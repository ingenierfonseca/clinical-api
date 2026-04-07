using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Request;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var invoices = await _invoiceRepository.GetAllPaginatedAsync(pageNumber, pageSize);
            return Ok(invoices);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var invoices = await _invoiceRepository.GetByIdAsync(id);
            return Ok(invoices);
        }

        [HttpGet("customer/{id:int}")]
        public async Task<IActionResult> GetInvoiceByCustomer(int id)
        {
            var invoices = await _invoiceRepository.GetInvoicesByCustomerAsync(id);
            return Ok(invoices);
        }

        [HttpGet("customer/{id:int}/payments")]
        public async Task<IActionResult> GetPaymentsByCustomer(int id)
        {
            var invoices = await _invoiceRepository.GetPaymentsByCustomer(id);
            return Ok(invoices);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var invoices = await _invoiceRepository.GetAllDashboardPaginatedAsync(pageNumber, pageSize, search);
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestInvoiceDto dto)
        {
            var result = await _invoiceRepository.CreateInvoiceAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
