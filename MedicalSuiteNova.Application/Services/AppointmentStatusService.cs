using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    internal class AppointmentStatusService(IUnitOfWork uow, IMapper mapper) : BaseService<AppointmentStatus>(uow, mapper, uow.AppointmentStatuses), IAppointmentStatusService
    {
    }
}
