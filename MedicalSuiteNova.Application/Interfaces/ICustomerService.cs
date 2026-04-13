
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ICustomerService : IBaseService<Customer>
    {
        public Task<List<CustomerDashboardDto>> GetDashboard();
        public Task<PagedResponse<Customer>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search);
    }
}
