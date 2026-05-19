
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ISessionPlanMasterService: IBaseService<SessionPlanMaster>
    {
        Task<Result<SessionPlanMasterDto>> AddAsync(RequestSessionPlanMaster dto);
        Task<Result<SessionPlanMasterDto>> ChangeStatus(RequestStatusSessionPlanMaster request);
        Task<List<SessionPlanMasterDto>> GetByCustomer(int id);
    }
}
