using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        public Task<Result<ResponseInvoiceDto>> CreateInvoiceAsync(RequestInvoiceDto dto);
        new Task<InvoiceDto?> GetByIdAsync(int id);
        public Task<PagedResponse<InvoiceDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<List<CustomerDashboardDto>> GetDashboard();
        public Task<PagedResponse<CustomerInvoiceDashboardDto>> GetAllDashboardPaginatedAsync(int pageNumber, int pageSize, string search);
        public Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id);
        public Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id);
    }
}
