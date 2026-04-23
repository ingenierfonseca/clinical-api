
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ExchangeRateRepository: BaseRepository<ExchangeRate>, IExchangeRateRepository
    {
        public ExchangeRateRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<decimal> GetLatestRate(int fromCurrencyId, int toCurrencyId)
        {
            return await GetFirstMappedAsync(
                predicate: r => r.FromCurrencyId == fromCurrencyId && r.ToCurrencyId == toCurrencyId && r.IsActive,
                orderBy: r => r.RateDate,
                selector: r => r.Rate
            );
        }
    }
}
