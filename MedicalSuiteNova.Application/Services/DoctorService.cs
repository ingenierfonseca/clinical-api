
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Doctor;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class DoctorService(IUnitOfWork uow, IMapper mapper) : BaseService<Doctor>(uow, mapper, uow.Doctors), IDoctorService
    {
        public async Task<PagedResponse<DoctorInfoDto>> GetAllAsync(int pageNumber, int pageSize, int? specialtyId)
        {
            return await _uow.Doctors.GetAllAsync<DoctorInfoDto>(
                pageNumber, 
                pageSize,
                d => (!specialtyId.HasValue || d.ServiceId == specialtyId),
                null,
                null,
                d => d.Staff!,
                d => d.Specialty!,
                d => d.Service!
            );
        }

        public async Task<Result<DoctorInfoDto>> GetInfo(int id)
        {
            var doctor = await _repository.FirstOrDefaultAsync(
                d => d.Id == id,
                d => d.Staff!,
                d => d.Specialty!,
                d => d.Service!
            );

            if (doctor == null)
                return Result<DoctorInfoDto>.Failure("El doctor solicitado no fue encontrado"); ;
            
            return Result<DoctorInfoDto>.Success(
                new DoctorInfoDto
                {
                    Id = id,
                    Title = doctor.Title,
                    FirstName = doctor.Staff!.FirstName,
                    LastName = doctor.Staff!.LastName,
                    BirthDate = doctor.Staff!.BirthDate,
                    Service = doctor.Service!.Name,
                    Specialty = doctor.Specialty!.Name,
                    Avatar = doctor.Staff?.Avatar,
                }
            );
        }

        public async Task<Result<DoctorDto>> AddAsync(CreateDoctorDto dto)
        {
            var resultValidate = await ValidateDependenciesAsync(0, dto);
            if (!resultValidate.IsSuccess)
                return Result<DoctorDto>.Failure(resultValidate.ErrorMessage);

            var doctor = await _uow.Doctors.AddAsync(_mapper.Map<Doctor>(dto));
            await _uow.CompleteAsync();

            return Result<DoctorDto>.Success(_mapper.Map<DoctorDto>(doctor));
        }

        public async Task<Result<DoctorDto>> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _uow.Doctors.FindAsync(id);
            if (doctor == null)
                return Result<DoctorDto>.Failure("Id no encontrado");

            var resultValidate = await ValidateDependenciesAsync(id, new CreateDoctorDto
            {
                ServiceId = dto.ServiceId,
                SpecialtyId = dto.SpecialtyId,
                StaffId = dto.StaffId,
                Title = dto.Title
            });

            if (!resultValidate.IsSuccess)
                return Result<DoctorDto>.Failure(resultValidate.ErrorMessage);

            _mapper.Map(dto, doctor);
            doctor.Id = id;

            await _uow.Doctors.UpdateAsync(doctor);
            await _uow.CompleteAsync();

            return Result<DoctorDto>.Success(_mapper.Map<DoctorDto>(doctor));
        }

        private async Task<Result<bool>> ValidateDependenciesAsync(int id, CreateDoctorDto dto)
        {
            if (!await _uow.Staff.ExistsAsync(dto.StaffId))
                return Result<bool>.Failure("El Empleado no es válido.");

            if (!await _uow.Services.ExistsAsync(dto.ServiceId))
                return Result<bool>.Failure("El servicio no es válido.");

            if (!await _uow.Specialties.ExistsAsync(dto.SpecialtyId))
                return Result<bool>.Failure("La especialidad no es válida.");

            var existDoctor = await _uow.Doctors.FirstOrDefaultAsync(d => d.StaffId == dto.StaffId);
            if (existDoctor != null && (id == 0 || (id != 0 && existDoctor.Id != id)))
                return Result<bool>.Failure("El empleado ya esta asignado a un doctor");

            return Result<bool>.Success(true);
        }
    }
}
