using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class AppointmentRepository: BaseRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context, IMapper mapper): base(context, mapper) {}

        public async Task<PagedResponse<AppointmentInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            /*var query = _context.Appointments
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
                .OrderByDescending(a => a.AppointmentDate);*/

            Expression<Func<Appointment, AppointmentInfoDto>> selector = a => new AppointmentInfoDto
            {
                Id = a.Id,
                AppointmentDate = a.AppointmentDate,
                PatientName = a.Patient!.getShortName(),
                DoctorName = a.Doctor!.getShortName(),
                TypeName = a.AppointmentType!.Name
            };
            return await GetAllAsync<AppointmentInfoDto>(
                pageNumber,
                pageSize,
                null,
                query => query.OrderByDescending(a => a.AppointmentDate),
                new Expression<Func<Appointment, object>>[]
                {
                    a => a.Patient!,
                    a => a.Doctor!,
                    a => a.AppointmentType!
                });
        }
    }
}
