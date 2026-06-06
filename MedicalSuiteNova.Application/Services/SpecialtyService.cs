
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class SpecialtyService(IUnitOfWork uow, IMapper mapper) : BaseService<Specialty>(uow, mapper, uow.Specialties), ISpecialtyService
    {
    }
}
