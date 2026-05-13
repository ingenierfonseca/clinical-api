
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class CustomerAccountLedgerRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<CustomerAccountLedger>(context, mapper), ICustomerAccountLedgerRepository
    {
        public async Task<decimal> GetLastBalanceByCustomerIdAsync(int  customerId)
        {
            return await GetFirstMappedAsync(
                predicate: c => c.CustomerId == customerId,
                orderBy: c => c.CreatedAt,
                selector: c => c.BalanceAfter
            );
        }
    }
}
