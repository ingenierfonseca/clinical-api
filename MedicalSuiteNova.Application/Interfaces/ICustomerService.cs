
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ICustomerService : IBaseService<Customer>
    {
        Task<List<CustomerDashboardDto>> GetDashboard();
        Task<ResponseImportResult> BulkImport(List<CustomerImportDto> dtos);
        Task<Result<string>> UploadAvatarAsync(int id, IFormFile file);
    }
}
