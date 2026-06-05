using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IAppointmentService : IBaseService<Appointment>
    {
        Task<AppointmentStats> GetStats(DateOnly startDate, DateOnly endDate);
        Task<PagedResponse<AppointmentInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<Result<AppointmentDto>> AddAsync(CreateAppointmentDto Dto);
        Task<Result<AppointmentDto>> UpdateAsync(int Id, AppointmentDto Dto);
    }
}
