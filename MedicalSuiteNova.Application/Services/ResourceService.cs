using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    internal class ResourceService(IUnitOfWork uow, IMapper mapper) : BaseService<Resource>(uow, mapper, uow.Resources), IResourceService
    {
    }
}
