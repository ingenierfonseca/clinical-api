using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentTermController : Controller
    {
        private readonly IPaymentTermService _paymentTermService;

        public PaymentTermController(IPaymentTermService paymentTermService)
        {
            _paymentTermService = paymentTermService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _paymentTermService.GetAllAsync(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _paymentTermService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PaymentTermDto p)
        {
            var result = await _paymentTermService.CreateAsync(p);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdatePaymentTermDto p)
        {
            var result = await _paymentTermService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
