using AutoMapper;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class PaymentRepository: BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}
