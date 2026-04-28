using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IInvoiceService : IBaseService<Invoice>
    {
        public Task<Result<ResponseInvoiceDto>> CreateInvoiceAsync(RequestInvoiceDto dto);
        public Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id);
        Task<List<CustomerDashboardDto>> GetDashboard();
        public Task<PagedResponse<CustomerInvoiceDashboardDto>> GetAllDashboardPaginatedAsync(int pageNumber, int pageSize, string search);
        public Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id);
        public Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id);
        public Task<Result<ResponseInvoiceDto>> UpdateAsync(int id, RequestInvoiceDto dto);
    }
}
