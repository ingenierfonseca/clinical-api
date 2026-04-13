using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IAppointmentService : IBaseService<Appointment>
    {
        public Task<Result<AppointmentDto>> AddAsync(AppointmentDto Dto);
        public Task<Result<AppointmentDto>> UpdateAsync(int Id, AppointmentDto Dto);
    }
}
