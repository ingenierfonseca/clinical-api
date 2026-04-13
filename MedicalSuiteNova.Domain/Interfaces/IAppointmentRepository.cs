using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<PagedResponse<AppointmentInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    }
}
