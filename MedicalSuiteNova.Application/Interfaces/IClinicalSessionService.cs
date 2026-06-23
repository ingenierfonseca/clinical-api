using MedicalSuiteNova.Domain.Dto.ClinicalSession;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IClinicalSessionService: IBaseService<ClinicalSession>
    {
        Task<Result<ClinicalSessionDto>> AddAsync(ClinicalSessionDto dto);
        Task<IEnumerable<ClinicalSessionShortInfoDto>> GetShortInfoByCustomer(int customerId);
        Task<List<ClinicalSession>> GetHistoryCustomer(int customerId);
    }
}
