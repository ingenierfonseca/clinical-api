using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class InvoiceDetailRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<InvoiceItem>(context, mapper), IInvoiceDetailRepository
    {
        public async Task DeleteByInvoiceIdAsync(int invoiceId)
        {
            var items = await _context.InvoiceDetails
                .Where(x => x.InvoiceId == invoiceId)
                .ExecuteDeleteAsync();
        }
    }
}
