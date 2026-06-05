
using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.RolePermission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize(Roles = AppRole.SuperAdmin)]
    [ApiController]
    [Route("api/role-permission")]
    public class RolePermissionController(IRolePermissionService rolePermissionService) : ControllerBase
    {
        private readonly IRolePermissionService _rolePermissionService = rolePermissionService;

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _rolePermissionService.GetAllAsync<RolePermissionDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{roleId:int}")]
        public async Task<IActionResult> GetAll(int roleId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _rolePermissionService.GetAllAsync<RolePermissionDto>(pageNumber, pageSize, r => r.RoleId == roleId);
            return Ok(items);
        }

        [HttpGet("{roleId:int}/{permissionId:int}")]
        public async Task<IActionResult> Get(int roleId, int permissionId)
        {
            var item = await _rolePermissionService.FindAsync(roleId, permissionId);
            if (item == null)
                return NotFound(new { status = 404, errors = new[] { "Asignación no encontrada" } });
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRolePermissionDto dto)
        {
            try
            {
                var result = await _rolePermissionService.AddAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return Conflict(new { status = 409, errors = new[] { ex.Message } });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = 404, errors = new[] { ex.Message } });
            }
        }

        [HttpDelete("{roleId:int}/{permissionId:int}")]
        public async Task<IActionResult> Delete(int roleId, int permissionId)
        {
            try
            {
                await _rolePermissionService.DeleteAsync(roleId, permissionId);
                return Ok(new { status = 200, message = "Asignación eliminada exitosamente" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { status = 404, errors = new[] { ex.Message } });
            }
        }
    }
}
