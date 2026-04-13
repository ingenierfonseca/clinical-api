using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var patients = await _paymentService.GetAllAsync(pageNumber, pageSize);
            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var patients = await _paymentService.FyndAsync(id);
            return Ok(patients);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Payment p)
        {
            var result = await _paymentService.CreatePaymentAsync(p);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });

            return Ok(result.Value);
        }
    }
}
