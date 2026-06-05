using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    internal class ResourceTypeService(IUnitOfWork uow, IMapper mapper) : BaseService<ResourceType>(uow, mapper, uow.ResourceTypes), IResourceTypeService
    {
    }
}
