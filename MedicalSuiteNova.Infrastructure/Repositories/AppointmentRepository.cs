using AutoMapper;
using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class AppointmentRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<Appointment>(context, mapper), IAppointmentRepository
    {
        public async Task<PagedResponse<AppointmentInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            Expression<Func<Appointment, AppointmentInfoDto>> selector = a => new AppointmentInfoDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                DoctorId = a.DoctorId,
                StatusId = a.StatusId,
                AppointmentTypeId = a.AppointmentTypeId,
                ResourceId = a.ResourceId,
                Date = a.Date,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                PatientName = a.Patient!.GetShortName(),
                DoctorName = a.Doctor!.Staff!.GetShortName(),
                TypeName = a.AppointmentType!.Name,
                StatusName = a.Status!.Name,
                ResourceName = a.Resource != null ? a.Resource.Name : null,
                Notes = a.Notes
            };
            return await GetAllAsync(
                pageNumber,
                pageSize,
                null,
                query => query.OrderByDescending(a => a.Date),
                selector,
                a => a.Patient!,
                a => a.Doctor!,
                a => a.AppointmentType!,
                a => a.Status!,
                a => a.Resource!
            );
        }
    }
}
