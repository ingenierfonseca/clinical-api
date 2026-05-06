
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ISessionPlanMasterService: IBaseService<SessionPlanMaster>
    {
        Task<Result<SessionPlanMasterDto>> AddAsync(SessionPlanMasterDto dto);
    }
}
