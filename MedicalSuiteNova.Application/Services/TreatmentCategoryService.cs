
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class TreatmentCategoryService(IUnitOfWork uow, IMapper mapper) : BaseService<TreatmentCategory>(uow, mapper, uow.TreatmentsCategory), ITreatmentCategoryService
    {
    }
}
