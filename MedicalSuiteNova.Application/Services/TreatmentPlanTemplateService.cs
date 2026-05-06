
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class TreatmentPlanTemplateService(IUnitOfWork uow, IMapper mapper): BaseService<TreatmentPlanTemplate>(uow, mapper, uow.TreatmentPlanTemplates), ITreatmentPlanTemplateService
    {
    }
}
