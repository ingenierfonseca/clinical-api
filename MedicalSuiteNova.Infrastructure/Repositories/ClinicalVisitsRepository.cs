
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ClinicalVisitsRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ClinicalVisits>(context, mapper), IClinicalVisitsRepository
    {
    }
}
