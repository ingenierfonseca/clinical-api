
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class TreatmentService: BaseService<Treatment>, ITreatmentService
    {
        public TreatmentService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Treatments) { }
    }
}
