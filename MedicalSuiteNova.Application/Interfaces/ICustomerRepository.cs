using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ICustomerRepository: IBaseRepository<Customer>
    {
        Task<PagedResponse<Customer>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search);
    }
}
