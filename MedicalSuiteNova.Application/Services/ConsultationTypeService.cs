
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class ConsultationTypeService(IUnitOfWork uow, IMapper mapper) : BaseService<ConsultationType>(uow, mapper, uow.ConsultationTypes), IConsultationTypeService
    {
    }
}
