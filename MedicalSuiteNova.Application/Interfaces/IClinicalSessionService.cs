
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IClinicalSessionService: IBaseService<ClinicalSession>
    {
        Task<Result<ClinicalSessionDto>> AddAsync(ClinicalSessionDto dto);
    }
}
