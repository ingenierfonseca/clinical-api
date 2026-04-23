
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class AppointmentService: BaseService<Appointment>, IAppointmentService
    {
        public AppointmentService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Appointments) { }

        public async Task<Result<AppointmentDto>> AddAsync(AppointmentDto dto)
        {
            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<AppointmentDto>.Failure(validation.ErrorMessage);

            var appointment = _mapper.Map<Appointment>(dto);
            var result = await _uow.Appointments.AddAsync(appointment);
            await _uow.CompleteAsync();

            var resultDto = _mapper.Map<AppointmentDto>(appointment);
            return Result<AppointmentDto>.Success(resultDto);
        }

        public async Task<Result<AppointmentDto>> UpdateAsync(int id, AppointmentDto dto)
        {
            var appointment = await _uow.Appointments.FindAsync(id);

            if (appointment == null)
                return Result<AppointmentDto>.Failure($"La cita con ID {id} no fue encontrada.");

            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<AppointmentDto>.Failure(validation.ErrorMessage);

            _mapper.Map(dto, appointment);
            appointment.Id = id;

            await _uow.Appointments.UpdateAsync(appointment);
            await _uow.CompleteAsync();

            return Result<AppointmentDto>.Success(_mapper.Map<AppointmentDto>(appointment));
        }

        private async Task<Result<bool>> ValidateDependenciesAsync(AppointmentDto dto)
        {
            if (!await _uow.Customers.ExistsAsync(dto.CustomerId))
                return Result<bool>.Failure("El Paciente no es válido.");

            if (!await _uow.Doctors.ExistsAsync(dto.DoctorId))
                return Result<bool>.Failure("El Doctor no es válido.");

            if (!await _uow.AppointmentTypes.ExistsAsync(dto.AppointmentTypeId))
                return Result<bool>.Failure("El Tipo de cita no es válido.");

            return Result<bool>.Success(true);
        }
    }
}
