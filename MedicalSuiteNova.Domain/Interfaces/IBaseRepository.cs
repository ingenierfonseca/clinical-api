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
        /*Task<PagedResponse<TDto>> GetAllAsync<TDto>(
            int pageNumber,
            int pageSize,
            Expression<Func<T, TDto>>[] includes
        ) where TDto : class;*/
        public Task<bool> ExistsAsync(object id);
        Task<T?> FyndAsync(int id);
        Task<T> AddAsync(T patient);
        Task<T> UpdateAsync(T t);
    }
}
