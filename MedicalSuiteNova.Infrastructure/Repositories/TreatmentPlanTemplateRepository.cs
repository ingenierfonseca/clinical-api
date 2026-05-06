
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class TreatmentPlanTemplateRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<TreatmentPlanTemplate>(context, mapper), ITreatmentPlanTemplateRepository
    {
    }
}
