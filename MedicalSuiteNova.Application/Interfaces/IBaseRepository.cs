using MedicalSuiteNova.Domain.Dto.Responses;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
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
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T patient);
        Task<T> UpdateAsync(T t);
    }
}
