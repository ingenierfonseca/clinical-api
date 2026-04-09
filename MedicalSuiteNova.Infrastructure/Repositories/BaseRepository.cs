using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponse<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            var query = _context.Set<T>();
            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<T>(data, pageNumber, pageSize, totalRecords);
        }

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(int pageNumber, int pageSize, Expression<Func<T, TDto>> selector) where TDto : class
        {
            var query = _context.Set<T>();
            var totalRecords = await query.CountAsync();

            var data = await query
                .Select(selector)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<TDto>(data, pageNumber, pageSize, totalRecords);
        }

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(int pageNumber, int pageSize, IQueryable<TDto> query) where TDto : class
        {
            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<TDto>(data, pageNumber, pageSize, totalRecords);
        }

        /*public async Task<PagedResponse<T>> GetAllAsync<TDto>(int pageNumber, int pageSize, Expression<Func<T, object>>[] includes) where TDto : class
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<T>(data, pageNumber, pageSize, totalRecords);
        }*/

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public async Task<T> UpdateAsync(T t)
        {
            _context.Set<T>().Update(t);
            await _context.SaveChangesAsync();
            return t;
        }
    }
}
