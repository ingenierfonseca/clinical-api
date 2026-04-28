using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = context.Set<T>();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet;
            if (predicate != null) query = query.Where(predicate);

            return await query.ToListAsync();
        }

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(int page, int size, Expression<Func<T, bool>> predicate) where TDto : class
            => await GetAllAsync<TDto>(page, size, predicate, null);

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes
        ) where TDto : class
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
                foreach (var include in includes) query = query.Include(include);

            if (predicate != null) query = query.Where(predicate);

            if (orderBy != null) query = orderBy(query);

            var totalRecords = await query.CountAsync();

            var data = await query
                .ProjectTo<TDto>(_mapper.ConfigurationProvider)
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

        public async Task<bool> ExistsAsync(object id)
        {
            return await _dbSet.AnyAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }

        public async Task<T?> FindAsync(int id)
        {
            var idProperty = typeof(T).GetProperty("Id");

            if (idProperty == null)
                return await _dbSet.FindAsync(id);

            var convertedId = Convert.ChangeType(id, idProperty.PropertyType);

            return await _dbSet.FindAsync(convertedId);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<TResult?> GetFirstMappedAsync<TResult>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>> orderBy,
            Expression<Func<T, TResult>> selector)
        {
            return await _dbSet
                .Where(predicate)
                .OrderByDescending(orderBy)
                .Select(selector)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
                query = query.Include(include);

            return await query.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> AddAsync(T t)
        {
            await _dbSet.AddAsync(t);
            return t;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<T> UpdateAsync(T t)
        {
            _dbSet.Update(t);
            return t;
        }
    }
}
