using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ClinicalFileRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ClinicalFile>(context, mapper), IClinicalFileRepository
    {
    }
}
