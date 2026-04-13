
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IBaseService<T> where T : class, IEntity
    {
        Task<PagedResponse<T>> GetAllAsync( int pageNumber, int pageSize);
        //Task<PagedResponse<T>> GetAllAsync( int pageNumber, int pageSize, string search);
        Task<T?> FyndAsync(int id);
        Task<T> AddAsync(T patient);
        Task<T> UpdateAsync(T t);
    }
}
