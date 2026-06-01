using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .Include(u => u.Doctor)
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .Include(u => u.Doctor)
                .Include(u => u.Customer)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task SetRolesAsync(int userId, List<int> roleIds)
        {
            var existing = await _context.Set<UserRole>()
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            _context.Set<UserRole>().RemoveRange(existing);

            foreach (var roleId in roleIds)
            {
                await _context.Set<UserRole>().AddAsync(new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                });
            }
        }
    }
}
