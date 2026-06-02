
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.RolePermission;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IRolePermissionService
    {
        Task<PagedResponse<RolePermissionDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<RolePermissionDto?> FindAsync(int roleId, int permissionId);
        Task<RolePermissionDto> AddAsync(CreateRolePermissionDto dto);
        Task DeleteAsync(int roleId, int permissionId);
    }
}
