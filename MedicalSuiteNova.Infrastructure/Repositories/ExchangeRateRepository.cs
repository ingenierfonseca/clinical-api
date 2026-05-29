
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ExchangeRateRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ExchangeRate>(context, mapper), IExchangeRateRepository
    {
        public async Task<decimal> GetLatestRate(int fromCurrencyId, int toCurrencyId)
        {
            return await GetFirstMappedAsync(
                predicate: r => r.FromCurrencyId == fromCurrencyId && r.ToCurrencyId == toCurrencyId && r.IsActive,
                orderBy: r => r.RateDate,
                selector: r => r.Rate
            );
        }

        public async Task InactivateActiveRatesAsync(int fromCurrencyId, int toCurrencyId)
        {
            var rates = await _dbSet
                .Where(e =>
                    e.FromCurrencyId == fromCurrencyId &&
                    e.ToCurrencyId == toCurrencyId &&
                    e.IsActive)
                .ToListAsync();

            foreach (var rate in rates)
            {
                rate.IsActive = false;
            }

            await _context.SaveChangesAsync();
        }
    }
}
