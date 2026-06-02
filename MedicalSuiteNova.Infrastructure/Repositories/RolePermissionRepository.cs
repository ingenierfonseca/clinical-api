
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RolePermissionRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResponse<RolePermission>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Set<RolePermission>()
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<RolePermission>(data, pageNumber, pageSize, totalRecords);
        }

        public async Task<RolePermission?> FindAsync(int roleId, int permissionId)
        {
            return await _context.Set<RolePermission>()
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        public async Task<bool> ExistsAsync(int roleId, int permissionId)
        {
            return await _context.Set<RolePermission>()
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }

        public async Task<RolePermission> AddAsync(RolePermission entity)
        {
            await _context.Set<RolePermission>().AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(RolePermission entity)
        {
            _context.Set<RolePermission>().Remove(entity);
        }
    }
}
