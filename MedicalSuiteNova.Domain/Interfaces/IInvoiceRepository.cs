using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id);
        Task<PagedResponse<CustomerInvoiceDashboardDto>> GetAllDashboardPaginatedAsync(int pageNumber, int pageSize, string search);
        Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id);
        Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id);
        Task<string> GetLastInvoiceNumberAsync();
    }
}
