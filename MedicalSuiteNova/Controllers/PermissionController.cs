using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize(Roles = AppRole.SuperAdmin)]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionController(IPermissionService permissionService) : ControllerBase
    {
        private readonly IPermissionService _permissionService = permissionService;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50)
        {
            var items = await _permissionService.GetAllAsync<PermissionDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var permission = await _permissionService.FindAsync(id);
            if (permission == null)
                return NotFound(new { status = 404, errors = new[] { "Permiso no encontrado" } });
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto)
        {
            var permission = await _permissionService.AddAsync(dto);
            return Ok(permission);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionDto dto)
        {
            var result = await _permissionService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }
    }
}
