using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Payment;
using MedicalSuiteNova.Domain.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController(IPaymentService paymentService) : Controller
    {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpGet]
        [Authorize(Policy = AppPolicies.CanViewPayments)]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _paymentService.GetAllAsync<PaymentDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPolicies.CanViewPayments)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _paymentService.FindAsync(id);
            return Ok(item);
        }

        [HttpGet("baucher/{id:int}")]
        [Authorize(Policy = AppPolicies.CanViewPayments)]
        public async Task<IActionResult> GetBaucher(int id)
        {
            var result = await _paymentService.GetBaucher(id);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }

        [HttpPost]
        [Authorize(Policy = AppPolicies.CanCreatePayments)]
        public async Task<IActionResult> Post(PaymentRequest p)
        {
            var result = await _paymentService.CreatePaymentAsync(p);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
