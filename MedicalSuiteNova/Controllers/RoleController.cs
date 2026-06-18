using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize(Roles = AppRole.SuperAdmin)]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var items = await _roleService.GetAllAsync<RoleDto>(pageNumber, pageSize);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.FindAsync(id);
            if (role == null)
                return NotFound(new { status = 404, errors = new[] { "Rol no encontrado" } });
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleDto dto)
        {
            var role = await _roleService.AddAsync<CreateRoleDto, RoleDto>(dto);
            return Ok(role);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateRoleDto dto)
        {
            var result = await _roleService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return BadRequest(new { message = result.ErrorMessage });
            return Ok(result.Value);
        }

        [HttpPut("assign-user-roles/{userId:int}")]
        public async Task<IActionResult> AssignUserRoles(int userId, [FromBody] AssignUserRolesDto dto)
        {
            await _roleService.AssignRolesToUserAsync(userId, dto.RoleIds);
            return Ok(new { status = 200, message = "Roles asignados al usuario exitosamente" });
        }

        [HttpPut("{roleId:int}/assign-permissions")]
        public async Task<IActionResult> AssignPermissions(int roleId, [FromBody] AssignPermissionsDto dto)
        {
            await _roleService.AssignPermissionsToRoleAsync(roleId, dto.PermissionIds);
            return Ok(new { status = 200, message = "Permisos asignados al rol exitosamente" });
        }

        [HttpGet("{roleId:int}/permissions")]
        public async Task<IActionResult> GetRolePermissions(int roleId)
        {
            var permissionIds = await _roleService.GetRolePermissionIdsAsync(roleId);
            return Ok(permissionIds);
        }
    }
}
