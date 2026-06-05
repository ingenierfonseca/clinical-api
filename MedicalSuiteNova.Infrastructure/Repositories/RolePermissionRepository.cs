
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class RolePermissionRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<RolePermission>(context, mapper), IRolePermissionRepository
    {
        public async Task DeleteAsync(RolePermission entity)
        {
            _context.Set<RolePermission>().Remove(entity);
        }
    }
}
