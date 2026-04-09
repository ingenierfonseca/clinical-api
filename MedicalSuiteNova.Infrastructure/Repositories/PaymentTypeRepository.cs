using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class PaymentTypeRepository : BaseRepository<PaymentType>
    {
        public PaymentTypeRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> IsValidPaymentTypeAsync(int paymentTypeId)
        {
            return await _context.PaymentTypes.AnyAsync(p => p.Id == paymentTypeId);
        }
    }
}
