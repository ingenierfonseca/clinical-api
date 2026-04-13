
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    internal class AppointmentTypeService : BaseService<AppointmentType>, IAppointmentTypeService
    {
        public AppointmentTypeService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.AppointmentTypes) { }
    }
}
