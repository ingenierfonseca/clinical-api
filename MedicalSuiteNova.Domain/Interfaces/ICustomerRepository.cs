using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface ICustomerRepository: IBaseRepository<Customer>
    {
        public Task<bool> IsValidAsync(int patientId);
        Task<PagedResponse<Customer>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search);
    }
}
