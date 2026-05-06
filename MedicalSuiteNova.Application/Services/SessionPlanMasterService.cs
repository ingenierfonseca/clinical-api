
using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class SessionPlanMasterService(IUnitOfWork uow, IMapper mapper) : BaseService<SessionPlanMaster>(uow, mapper, uow.SessionPlanMaster), ISessionPlanMasterService
    {
        public async Task<Result<SessionPlanMasterDto>> AddAsync(SessionPlanMasterDto dto)
        {
            if (!await _uow.ClinicalSessions.ExistsAsync(dto.SessionId))
                return Result<SessionPlanMasterDto>.Failure("El SessionId no es válido.");

            if (!await _uow.Currencies.ExistsAsync(dto.CurrencyId))
                return Result<SessionPlanMasterDto>.Failure("El CurrencyId no es válido.");

            if (dto.Items == null || dto.Items.Count == 0)
                return Result<SessionPlanMasterDto>.Failure("El detalle del plan es requerido.");

            foreach (var item in dto.Items)
            {
                if (!await _uow.TreatmentsPlanTemplateItems.ExistsAsync(item.TreatmentPlanTemplateItemId))
                    return Result<SessionPlanMasterDto>.Failure("El TreatmentPlanTemplateItemId no es válido.");
                item.Status = PlanStatus.Pending;
            }

            try
            {
                dto.Status = PlanStatus.Pending;
                var session = _mapper.Map<SessionPlanMaster>(dto);
                var result = await _uow.SessionPlanMaster.AddAsync(session);
                await _uow.CompleteAsync();

                return Result<SessionPlanMasterDto>.Success(_mapper.Map<SessionPlanMasterDto>(result));
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                return Result<SessionPlanMasterDto>.Failure("Ocurrió un error inesperado al procesar el plan.");
            }
        }
    }
}
