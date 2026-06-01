using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task SetPermissionsAsync(int roleId, List<int> permissionIds);
        Task<List<int>> GetPermissionIdsAsync(int roleId);
    }
}
