
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class PaymentTermRepository: BaseRepository<PaymentTerm>, IPaymentTermRepository
    {
        public PaymentTermRepository(ApplicationDbContext context, IMapper mapper): base(context, mapper) { }
    }
}
