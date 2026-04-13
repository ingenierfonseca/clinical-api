using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        public Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id);
        public IQueryable<InvoiceItemInfoDto> GetInvoicesAsQueryable();
        public Task<PagedResponse<CustomerInvoiceDashboardDto>> GetAllDashboardPaginatedAsync(int pageNumber, int pageSize, string search);
        public Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id);
        public Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id);
    }
}
