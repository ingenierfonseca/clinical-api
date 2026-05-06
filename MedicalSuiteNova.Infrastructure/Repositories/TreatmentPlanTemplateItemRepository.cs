
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class TreatmentPlanTemplateItemRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<TreatmentPlanTemplateItem>(context, mapper), ITreatmentPlanTemplateItemRepository
    {
    }
}
