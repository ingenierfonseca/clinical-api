using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.Roles)
        {
        }

        public async Task AssignRolesToUserAsync(int userId, List<int> roleIds)
        {
            var user = await _uow.Users.FindAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Usuario no encontrado");

            foreach (var roleId in roleIds)
            {
                var role = await _uow.Roles.FindAsync(roleId);
                if (role == null)
                    throw new KeyNotFoundException($"Rol con ID {roleId} no encontrado");

                if (!role.IsActive)
                    throw new ArgumentException($"El rol '{role.Name}' está inactivo");
            }

            await _uow.Users.SetRolesAsync(userId, roleIds);
            await _uow.CompleteAsync();
        }

        public async Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            var role = await _uow.Roles.FindAsync(roleId);
            if (role == null)
                throw new KeyNotFoundException("Rol no encontrado");

            foreach (var permissionId in permissionIds)
            {
                var permission = await _uow.Permissions.FindAsync(permissionId);
                if (permission == null)
                    throw new KeyNotFoundException($"Permiso con ID {permissionId} no encontrado");
            }

            await _uow.Roles.SetPermissionsAsync(roleId, permissionIds);
            await _uow.CompleteAsync();
        }

        public async Task<List<int>> GetRolePermissionIdsAsync(int roleId)
        {
            return await _uow.Roles.GetPermissionIdsAsync(roleId);
        }
    }
}
