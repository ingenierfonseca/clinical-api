
using MedicalSuiteNova.Domain.Dto.Doctor;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IDoctorService: IBaseService<Doctor>
    {
        Task<Result<DoctorInfoDto>> GetInfo(int id);
        Task<PagedResponse<DoctorInfoDto>> GetAllAsync(int pageNumber, int pageSize, int? specialtyId);
        Task<Result<DoctorDto>> AddAsync(CreateDoctorDto dto);
        Task<Result<DoctorDto>> UpdateAsync(int id, UpdateDoctorDto dto);
    }
}
