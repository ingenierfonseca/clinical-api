
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface ICustomerAccountLedgerRepository: IBaseRepository<CustomerAccountLedger>
    {
        Task<decimal> GetLastBalanceByCustomerIdAsync(int customerId);
    }
}
