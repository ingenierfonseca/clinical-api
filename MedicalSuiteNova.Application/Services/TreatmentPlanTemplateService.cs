
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class TreatmentPlanTemplateService(IUnitOfWork uow, IMapper mapper): BaseService<TreatmentPlanTemplate>(uow, mapper, uow.TreatmentPlanTemplates), ITreatmentPlanTemplateService
    {
        public async Task<Result<TreatmentPlanTemplateDto>> AddAsync(TreatmentPlanTemplateDto dto)
        {
            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<TreatmentPlanTemplateDto>.Failure(validation.ErrorMessage);

            var planTemplate = _mapper.Map<TreatmentPlanTemplate>(dto);
            await _uow.TreatmentPlanTemplates.AddAsync(planTemplate);
            await _uow.CompleteAsync();

            return Result<TreatmentPlanTemplateDto>.Success(_mapper.Map<TreatmentPlanTemplateDto>(planTemplate));
        }

        public async Task<Result<TreatmentPlanTemplateDto>> UpdateAsync(int id, TreatmentPlanTemplateDto dto)
        {
            var appointment = await _uow.Appointments.FindAsync(id);

            if (appointment == null)
                return Result<TreatmentPlanTemplateDto>.Failure($"La cita con ID {id} no fue encontrada.");

            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<TreatmentPlanTemplateDto>.Failure(validation.ErrorMessage);

            _mapper.Map(dto, appointment);
            appointment.Id = id;

            await _uow.Appointments.UpdateAsync(appointment);
            await _uow.CompleteAsync();

            return Result<TreatmentPlanTemplateDto>.Success(_mapper.Map<TreatmentPlanTemplateDto>(appointment));
        }

        private async Task<Result<bool>> ValidateDependenciesAsync(TreatmentPlanTemplateDto dto)
        {
            if (!await _uow.TreatmentsCategory.ExistsAsync(dto.CategoryId))
                return Result<bool>.Failure("La Categoria no es válido.");

            if (!await _uow.Currencies.ExistsAsync(dto.CurrencyId))
                return Result<bool>.Failure("El tipo de moneda no es válido.");

            if (dto.Items == null || dto.Items.Count == 0)
                return Result<bool>.Failure("Debe agregar al menos un item.");

            return Result<bool>.Success(true);
        }
    }
}
