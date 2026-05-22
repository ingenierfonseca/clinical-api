
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class DoctorService(IUnitOfWork uow, IMapper mapper) : BaseService<Doctor>(uow, mapper, uow.Doctors), IDoctorService
    {
    }
}
