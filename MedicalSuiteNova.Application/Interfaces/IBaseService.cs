
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Interfaces;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IBaseService<T> where T : class, IEntity
    {
        Task<PagedResponse<T>> GetAllAsync( int pageNumber, int pageSize);
        Task<PagedResponse<Dto>> GetAllAsync<Dto>(int pageNumber, int pageSize, Expression<Func<T, object>>[] includes) where Dto : class;
        //Task<PagedResponse<T>> GetAllAsync( int pageNumber, int pageSize, string search);
        Task<T?> FindAsync(int id);
        Task<T> AddAsync(T patient);
        public Task<Dto> AddAsync<Dto>(Dto dto) where Dto : class;
        Task<Result<Dto>> UpdateAsync<Dto>(int id, Dto t) where Dto : class;
    }
}
