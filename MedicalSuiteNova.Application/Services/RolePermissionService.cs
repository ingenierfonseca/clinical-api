
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.RolePermission;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class RolePermissionService(IUnitOfWork uow, IMapper mapper) : BaseService<RolePermission>(uow, mapper, uow.RolePermissions), IRolePermissionService
    {
        public async Task<RolePermissionDto?> FindAsync(int roleId, int permissionId)
        {
            var entity = await _uow.RolePermissions.FirstOrDefaultAsync(p => p.RoleId == roleId && p.PermissionId == permissionId);
            return entity == null ? null : _mapper.Map<RolePermissionDto>(entity);
        }

        public async Task<RolePermissionDto> AddAsync(CreateRolePermissionDto dto)
        {
            var exists = await _uow.RolePermissions.FirstOrDefaultAsync(p => p.RoleId == dto.RoleId && p.PermissionId == dto.PermissionId);
            if (exists != null)
                throw new ArgumentException("La asignación ya existe");

            var role = await _uow.Roles.FindAsync(dto.RoleId)
                ?? throw new KeyNotFoundException($"Rol con ID {dto.RoleId} no encontrado");
            var permission = await _uow.Permissions.FindAsync(dto.PermissionId)
                ?? throw new KeyNotFoundException($"Permiso con ID {dto.PermissionId} no encontrado");

            var entity = new RolePermission
            {
                RoleId = dto.RoleId,
                PermissionId = dto.PermissionId,
                Role = role,
                Permission = permission
            };

            await _uow.RolePermissions.AddAsync(entity);
            await _uow.CompleteAsync();

            return _mapper.Map<RolePermissionDto>(entity);
        }

        public async Task DeleteAsync(int roleId, int permissionId)
        {
            var entity = await _uow.RolePermissions.FirstOrDefaultAsync(r => r.RoleId == roleId && r.PermissionId == permissionId)
                ?? throw new KeyNotFoundException("Asignación no encontrada");

            await _uow.RolePermissions.DeleteAsync(entity);
            await _uow.CompleteAsync();
        }
    }
}
