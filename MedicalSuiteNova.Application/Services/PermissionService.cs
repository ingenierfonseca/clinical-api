using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class PermissionService(IUnitOfWork uow, IMapper mapper) : BaseService<Permission>(uow, mapper, uow.Permissions), IPermissionService
    {
    }
}
