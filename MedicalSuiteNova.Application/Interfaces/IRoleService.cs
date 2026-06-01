using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IRoleService : IBaseService<Role>
    {
        Task AssignRolesToUserAsync(int userId, List<int> roleIds);
        Task AssignPermissionsToRoleAsync(int roleId, List<int> permissionIds);
        Task<List<int>> GetRolePermissionIdsAsync(int roleId);
    }
}
