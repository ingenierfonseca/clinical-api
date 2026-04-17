using AutoMapper;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;

        public BaseRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
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

        public async Task<PagedResponse<T>> GetAllAsync(
            int pageNumber,
            int pageSize,
            params Expression<Func<T, object>>[] includes
        )
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<T>(data, pageNumber, pageSize, totalRecords);
        }

        public async Task<bool> ExistsAsync(object id)
        {
            return await _context.Set<T>().AnyAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public async Task<T?> FindAsync(int id)
        {
            var idProperty = typeof(T).GetProperty("Id");

            if (idProperty == null)
                return await _context.Set<T>().FindAsync(id);

            var convertedId = Convert.ChangeType(id, idProperty.PropertyType);

            return await _context.Set<T>().FindAsync(convertedId);
        }

        public async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> AddAsync(T t)
        {
            await _context.Set<T>().AddAsync(t);
            return t;
        }

        public async Task<T> UpdateAsync(T t)
        {
            _context.Set<T>().Update(t);
            return t;
        }
    }
}
