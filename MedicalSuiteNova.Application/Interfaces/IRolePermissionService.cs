
using MedicalSuiteNova.Domain.Dto.RolePermission;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IRolePermissionService: IBaseService<RolePermission>
    {
        Task<RolePermissionDto?> FindAsync(int roleId, int permissionId);
        Task<RolePermissionDto> AddAsync(CreateRolePermissionDto dto);
        Task DeleteAsync(int roleId, int permissionId);
    }
}
