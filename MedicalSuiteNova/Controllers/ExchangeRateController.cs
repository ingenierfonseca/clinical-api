using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController : Controller
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _exchangeRateService.GetAllAsync<ExchangeRate>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var items = await _exchangeRateService.FindAsync(id);
            return Ok(items);
        }

        [HttpGet("latest/{from:int}/{to:int}")]
        public async Task<IActionResult> GetLatestRate(int from, int to)
        {
            var result = await _exchangeRateService.GetLatestRate(from, to);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Post(PaymentTermDto p)
        {
            var result = await _exchangeRateService.AddAsync(p);
            //if (!result.IsSuccess)
                //return BadRequest(new { message = result.ErrorMessage });

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdatePaymentTermDto p)
        {
            var result = await _exchangeRateService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
