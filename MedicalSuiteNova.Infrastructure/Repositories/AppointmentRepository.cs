using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class AppointmentRepository: BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context): base(context)
        {
            
        }

        public async Task<PagedResponse<AppointmentDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentType)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    PatientName = a.Patient.getShortName(),
                    DoctorName = a.Doctor.getShortName(),
                    TypeName = a.AppointmentType.Name
                })
                .OrderByDescending(a => a.AppointmentDate);
            return await GetAllAsync(pageNumber, pageSize, query);
        }
    }
}
