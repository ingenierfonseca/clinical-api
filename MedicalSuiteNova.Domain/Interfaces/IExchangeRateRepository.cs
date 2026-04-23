
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IExchangeRateRepository:IBaseRepository<ExchangeRate>
    {
        public Task<decimal> GetLatestRate(int fromCurrencyId, int toCurrencyId);
    }
}
