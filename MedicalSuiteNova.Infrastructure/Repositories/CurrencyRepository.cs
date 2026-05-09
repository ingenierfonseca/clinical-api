
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class CurrencyRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<Currency>(context, mapper), ICurrencyRepository
    {
    }
}
