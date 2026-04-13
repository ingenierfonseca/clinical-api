
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class DoctorService: BaseService<Doctor>, IDoctorService
    {
        public DoctorService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Doctors) { }
    }
}
