
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IRolePermissionRepository
    {
        Task<PagedResponse<RolePermission>> GetAllAsync(int pageNumber, int pageSize);
        Task<RolePermission?> FindAsync(int roleId, int permissionId);
        Task<bool> ExistsAsync(int roleId, int permissionId);
        Task<RolePermission> AddAsync(RolePermission entity);
        Task DeleteAsync(RolePermission entity);
    }
}
