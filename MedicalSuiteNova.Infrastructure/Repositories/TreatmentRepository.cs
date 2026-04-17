
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class TreatmentRepository: BaseRepository<Treatment>, ITreatmentRepository
    {
        public TreatmentRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}
