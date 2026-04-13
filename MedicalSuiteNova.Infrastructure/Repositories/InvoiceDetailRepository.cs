using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using AutoMapper;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class InvoiceDetailRepository: BaseRepository<InvoiceItem>, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}
