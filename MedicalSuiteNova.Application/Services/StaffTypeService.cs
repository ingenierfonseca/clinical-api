
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    internal class StaffTypeService(IUnitOfWork uow, IMapper mapper) : BaseService<StaffType>(uow, mapper, uow.StaffTypes), IStaffTypeService
    {
    }
}
