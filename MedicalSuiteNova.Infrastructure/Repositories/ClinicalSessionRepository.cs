
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ClinicalSessionRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ClinicalSession>(context, mapper), IClinicalSessionRepository
    {
    }
}
