using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeRateController(IExchangeRateService exchangeRateService) : Controller
    {
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _exchangeRateService.GetAllAsync<ExchangeRateDto>(pageNumber, pageSize, null, q => q.OrderByDescending(e => e.RateDate), e => e.FromCurrency!, e => e.ToCurrency!);
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
        public async Task<IActionResult> Post(ExchangeRateDto p)
        {
            var result = await _exchangeRateService.AddAsync(p);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, ExchangeRateDto p)
        {
            var result = await _exchangeRateService.UpdateAsync(id, p);

            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return Ok(result.Value);
        }
    }
}
