
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ConsultationTypeRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<ConsultationType>(context, mapper), IConsultationTypeRepository
    {
    }
}
