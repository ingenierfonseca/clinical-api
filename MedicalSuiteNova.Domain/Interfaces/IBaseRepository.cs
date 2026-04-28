using MedicalSuiteNova.Domain.Dto.Responses;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<PagedResponse<TDto>> GetAllAsync<TDto>(int page, int size, Expression<Func<T, bool>> predicate) where TDto : class;
        Task<PagedResponse<TDto>> GetAllAsync<TDto>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes
        ) where TDto : class;

        Task<PagedResponse<TDto>> GetAllAsync<TDto>(int pageNumber, int pageSize, IQueryable<TDto> query) where TDto : class;
        Task<bool> ExistsAsync(object id);
        Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);
        Task<T?> FindAsync(int id);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T> AddAsync(T patient);
        Task<T> UpdateAsync(T t);
    }
}
