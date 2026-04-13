using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class AppointmentRepository: BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context, IMapper mapper): base(context, mapper) {}

        public async Task<PagedResponse<AppointmentInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentType)
                .Select(a => new AppointmentInfoDto
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    PatientName = a.Patient!.getShortName(),
                    DoctorName = a.Doctor!.getShortName(),
                    TypeName = a.AppointmentType!.Name
                })
                .OrderByDescending(a => a.AppointmentDate);
            return await GetAllAsync(pageNumber, pageSize, query);
        }
    }
}
