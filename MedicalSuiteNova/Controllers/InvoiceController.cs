using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var invoices = await _invoiceService.GetAllAsync<InvoiceItemInfoDto>(pageNumber, pageSize, null, query => query.OrderBy(a => a.CreatedAt));
            return Ok(invoices);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var invoices = await _invoiceService.GetByIdDtoAsync(id);
            return Ok(invoices);
        }

        [HttpGet("customer/{id:int}")]
        public async Task<IActionResult> GetInvoiceByCustomer(int id)
        {
            var invoices = await _invoiceService.GetInvoicesByCustomerAsync(id);
            return Ok(invoices);
        }

        [HttpGet("customer/{id:int}/payments")]
        public async Task<IActionResult> GetPaymentsByCustomer(int id)
        {
            var invoices = await _invoiceService.GetPaymentsByCustomer(id);
            return Ok(invoices);
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Get()
        {
            var invoices = await _invoiceService.GetDashboard();
            return Ok(invoices);
        }

        [HttpGet("dashboard-customers")]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = "")
        {
            var invoices = await _invoiceService.GetAllDashboardPaginatedAsync(pageNumber, pageSize, search);
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RequestInvoiceDto dto)
        {
            var result = await _invoiceService.CreateInvoiceAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, RequestInvoiceDto dto)
        {
            var result = await _invoiceService.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
