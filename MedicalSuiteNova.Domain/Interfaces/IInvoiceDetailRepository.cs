using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IInvoiceDetailRepository : IBaseRepository<InvoiceItem>
    {
        Task DeleteByInvoiceIdAsync(int invoiceId);
    }
}
