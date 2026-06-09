using MedicalSuiteNova.Domain.Dto.Customer;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<List<CustomerDashboardDto>> GetDashboard();
        Task<Result<CustomerDto>> AddAsync(CreateCustomerDto dto);
        Task<Result<CustomerDto>> UpdateAsync(int id, UpdateCustomerDto dto);
        Task<ResponseImportResult> BulkImport(List<CustomerImportDto> dtos);
        Task<Result<string>> UploadAvatarAsync(int id, IFormFile file);
    }
}
