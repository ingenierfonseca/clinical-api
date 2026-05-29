using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/treatment-category")]
    public class TreatmentCategoryController(ITreatmentCategoryService service) : Controller
    {
        private readonly ITreatmentCategoryService _service = service;

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _service.GetAllAsync<TreatmentCategoryDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TreatmentCategoryDto p)
        {
            var result = await _service.AddAsync(p);

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, TreatmentCategoryDto p)
        {
            var result = await _service.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
