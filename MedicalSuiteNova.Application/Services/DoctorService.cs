
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Doctor;
using MedicalSuiteNova.Domain.Dto.Responses;
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
                d => d.Staff!
            );
        }

        new public async Task<Result<DoctorInfoDto>> FindAsync(int id)
        {
            var doctor = await _repository.FirstOrDefaultAsync(
                d => d.Id == id,
                d => d.Staff!
            );

            if (doctor == null)
                return Result<DoctorInfoDto>.Failure("El doctor solicitado no fue encontrado"); ;
            return Result<DoctorInfoDto>.Success(
                new DoctorInfoDto
                {
                    Id = id,
                    FirstName = doctor.Staff!.FirstName,
                    LastName = doctor.Staff!.LastName,
                }
            );
        }
    }
}
