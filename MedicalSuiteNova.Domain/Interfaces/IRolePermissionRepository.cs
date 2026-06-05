
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IRolePermissionRepository:IBaseRepository<RolePermission>
    {
        Task DeleteAsync(RolePermission entity);
    }
}
