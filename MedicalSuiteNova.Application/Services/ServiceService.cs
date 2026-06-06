
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class ServiceService(IUnitOfWork uow, IMapper mapper) : BaseService<Service>(uow, mapper, uow.Services), IServiceService
    {
    }
}
