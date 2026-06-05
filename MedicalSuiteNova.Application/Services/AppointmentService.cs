using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class AppointmentService(IUnitOfWork uow, IMapper mapper) : BaseService<Appointment>(uow, mapper, uow.Appointments), IAppointmentService
    {
        public async Task<AppointmentStats> GetStats(DateOnly startDate, DateOnly endDate)
        {
            if (startDate == default)
                startDate = DateOnly.FromDateTime(DateTime.Today);
            if (endDate == default)
                endDate = DateOnly.FromDateTime(DateTime.Today);

            var allApointments = await _uow.Appointments.GetAllAsync(a => a.Date >= startDate && a.Date >= endDate);
            
            return new AppointmentStats
            {
                Total = allApointments.Count,
                Pending = allApointments.Where(a => a.StatusId == (byte)AppointmentStatusEnum.Pending).ToList().Count,
                Confirmed = allApointments.Where(a => a.StatusId == (byte)AppointmentStatusEnum.Confirmed).ToList().Count,
                Cancelled = allApointments.Where(a => a.StatusId == (byte)AppointmentStatusEnum.Cancelled).ToList().Count,
            };
        }
        public async Task<PagedResponse<AppointmentInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            return await _uow.Appointments.GetAllPaginatedAsync(pageNumber, pageSize);
        }

        public async Task<Result<AppointmentDto>> AddAsync(CreateAppointmentDto dto)
        {
            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<AppointmentDto>.Failure(validation.ErrorMessage);

            var endTime = validation.Value;

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.EndTime = endTime;
            appointment.StatusId = (byte)AppointmentStatusEnum.Pending;
            appointment.CreatedAt = DateTime.UtcNow;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _uow.Appointments.AddAsync(appointment);
            await _uow.CompleteAsync();

            var resultDto = _mapper.Map<AppointmentDto>(appointment);
            return Result<AppointmentDto>.Success(resultDto);
        }

        public async Task<Result<AppointmentDto>> UpdateAsync(int id, AppointmentDto dto)
        {
            var appointment = await _uow.Appointments.FindAsync(id);
            if (appointment == null)
                return Result<AppointmentDto>.Failure($"La cita con ID {id} no fue encontrada.");

            if (appointment.StatusId == (int)AppointmentStatusEnum.Completed ||
                appointment.StatusId == (int)AppointmentStatusEnum.Cancelled ||
                appointment.StatusId == (int)AppointmentStatusEnum.NoShow)
                return Result<AppointmentDto>.Failure("No se puede modificar una cita en estado finalizado, cancelado o No-Show.");

            if (appointment.StatusId != (int)AppointmentStatusEnum.Rescheduled && appointment.CustomerId != dto.CustomerId)
                return Result<AppointmentDto>.Failure("Tiene que cambiar el estado a 'Reagendada' para poder reasignar a un paciente diferente.");

            var createDto = _mapper.Map<CreateAppointmentDto>(dto);
            var validation = await ValidateDependenciesAsync(createDto, id);
            if (!validation.IsSuccess)
                return Result<AppointmentDto>.Failure(validation.ErrorMessage);

            var endTime = validation.Value;

            _mapper.Map(dto, appointment);
            appointment.Id = id;
            appointment.EndTime = endTime;
            appointment.UpdatedAt = DateTime.UtcNow;


            await _uow.Appointments.UpdateAsync(appointment);
            await _uow.CompleteAsync();

            return Result<AppointmentDto>.Success(_mapper.Map<AppointmentDto>(appointment));
        }

        private async Task<Result<TimeSpan>> ValidateDependenciesAsync(CreateAppointmentDto dto, int? id = null)
        {
            if (!await _uow.Customers.ExistsAsync(dto.CustomerId))
                return Result<TimeSpan>.Failure("El Paciente no es válido.");

            if (!await _uow.Doctors.ExistsAsync(dto.DoctorId))
                return Result<TimeSpan>.Failure("El Doctor no es válido.");

            if (dto.ResourceId.HasValue && !await _uow.Resources.ExistsAsync(dto.ResourceId.Value))
                return Result<TimeSpan>.Failure("El Recurso no es válido.");

            var appointmentType = await _uow.AppointmentTypes.FindAsync(dto.AppointmentTypeId);
            if (appointmentType == null)
                return Result<TimeSpan>.Failure("El Tipo de cita no fue encontrado.");

            var endTime = dto.StartTime.Add(TimeSpan.FromMinutes(appointmentType.DurationMinutes));

            if (await HasScheduleConflictAsync(dto, endTime, id))
                return Result<TimeSpan>.Failure("El horario ya se encuentra ocupado.");

            return Result<TimeSpan>.Success(endTime);
        }

        private async Task<bool> HasScheduleConflictAsync(CreateAppointmentDto dto, TimeSpan endTime, int? id = null)
        {
            return await _uow.Appointments.AnyAsync(a =>
                a.Date == dto.Date &&
                a.StartTime < endTime &&
                a.EndTime > dto.StartTime &&
                (id == null || a.Id != id) &&
                (a.DoctorId == dto.DoctorId || (dto.ResourceId != null && a.ResourceId == dto.ResourceId))
            );
        }
    }
}
