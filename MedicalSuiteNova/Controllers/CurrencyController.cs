using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController(ICurrencyService currencyService) : Controller
    {
        private readonly ICurrencyService _currencyService = currencyService;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _currencyService.GetAllAsync<CurrencyDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _currencyService.GetAllAsync<CurrencyDto>(
                pageNumber, 
                pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _currencyService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CurrencyDto c)
        {
            var result = await _currencyService.AddAsync(c);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CurrencyDto c)
        {
            var result = await _currencyService.UpdateAsync(id, c);

            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
             
            return Ok(result.Value);
        }
    }
}
