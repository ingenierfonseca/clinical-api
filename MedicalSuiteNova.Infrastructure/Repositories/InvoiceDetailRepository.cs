using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class InvoiceDetailRepository: BaseRepository<InvoiceItem>, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(ApplicationDbContext context) : base(context) { }
    }
}
