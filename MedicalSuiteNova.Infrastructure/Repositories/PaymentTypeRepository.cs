using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class PaymentTypeRepository : BaseRepository<PaymentType>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<bool> IsValidPaymentTypeAsync(int paymentTypeId)
        {
            return await _context.PaymentTypes.AnyAsync(p => p.Id == paymentTypeId);
        }
    }
}
