using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IInvoiceService : IBaseService<Invoice>
    {
        Task<Result<ResponseInvoiceDto>> CreateInvoiceAsync(RequestInvoiceDto dto);
        Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id);
        Task<List<CustomerDashboardDto>> GetDashboard();
        Task<PagedResponse<CustomerInvoiceDashboardDto>> GetAllDashboardPaginatedAsync(int pageNumber, int pageSize, string search);
        Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id);
        Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id);
        Task<Result<ResponseInvoiceDto>> UpdateAsync(int id, RequestInvoiceDto dto);
        Task<string> GenerateInvoiceNumberAsync();
    }
}
