using MedicalSuiteNova.Domain.Dto.Responses;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<PagedResponse<T>> GetAllAsync(
            int pageNumber,
            int pageSize
        );
        Task<PagedResponse<TDto>> GetAllAsync<TDto>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TDto>> selector
        ) where TDto : class;
        Task<PagedResponse<TDto>> GetAllAsync<TDto>(
            int pageNumber,
            int pageSize,
            IQueryable<TDto> query
        ) where TDto : class;
        Task<PagedResponse<T>> GetAllAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, object>>[] includes
        );
        public Task<bool> ExistsAsync(object id);
        public Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes);
        Task<T?> FindAsync(int id);
        public Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T patient);
        Task<T> UpdateAsync(T t);
    }
}
