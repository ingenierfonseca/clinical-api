using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ClinicalNotesRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ClinicalNotes>(context, mapper), IClinicalNotesRepository
    {
    }
}
