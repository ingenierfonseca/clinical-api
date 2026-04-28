
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ITreatmentService: IBaseService<Treatment>
    {
        public Task<Result<TreatmentDto>> CreateAsync(TreatmentDto dto);
    }
}
