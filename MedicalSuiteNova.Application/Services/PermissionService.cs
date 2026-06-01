using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class PermissionService : BaseService<Permission>, IPermissionService
    {
        public PermissionService(IUnitOfWork uow, IMapper mapper)
            : base(uow, mapper, uow.Permissions)
        {
        }
    }
}
