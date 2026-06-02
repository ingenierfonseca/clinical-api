
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class StaffService(IUnitOfWork uow, IMapper mapper) : BaseService<Staff>(uow, mapper, uow.Staff), IStaffService
    {
    }
}
