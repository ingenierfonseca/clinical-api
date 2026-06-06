
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class SpecialtyRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<Specialty>(context, mapper), ISpecialtyRepository
    {
    }
}
