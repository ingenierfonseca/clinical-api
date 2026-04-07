using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IAppointmentRepository : IBaseRepository<Appointment>
    {
        Task<PagedResponse<AppointmentDto>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    }
}
