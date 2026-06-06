
using MedicalSuiteNova.Domain.Dto.Doctor;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IDoctorService: IBaseService<Doctor>
    {
        new Task<Result<DoctorInfoDto>> FindAsync(int id);
        Task<PagedResponse<DoctorInfoDto>> GetAllAsync(int pageNumber, int pageSize, int? specialtyId);
    }
}
