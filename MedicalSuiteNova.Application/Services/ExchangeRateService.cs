
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class ExchangeRateService(IUnitOfWork uow, IMapper mapper) : BaseService<ExchangeRate>(uow, mapper, uow.ExchangeRates), IExchangeRateService
    {
        public async Task<Result<ExchangeRate>> GetLatestRate(int from, int to)
        {
            var result = await _uow.ExchangeRates.FirstOrDefaultAsync(x => x.FromCurrencyId == from && x.ToCurrencyId == to && x.IsActive);

            if (result == null)
                return Result<ExchangeRate>.Failure("No se encontro el tipo de cambio para los datos ingresados");

            else return Result<ExchangeRate>.Success(result);
        }

        public async Task<Result<ExchangeRateDto>> AddAsync(ExchangeRateDto dto)
        {
            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<ExchangeRateDto>.Failure(validation.ErrorMessage);
            dto.RateDate = DateTime.UtcNow;
            dto.Source = "Manual";

            var exchange = _mapper.Map<ExchangeRate>(dto);

            await _uow.BeginTransactionAsync();
            try
            {
                await InactivateActiveRatesAsync(dto);
                await _uow.ExchangeRates.AddAsync(exchange);
                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return Result<ExchangeRateDto>.Success(_mapper.Map<ExchangeRateDto>(exchange));
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                // Loguear internamente el error real para soporte técnico antes de enmascarar la respuesta
                Console.WriteLine($"[ClinicalSuiteNova Error]: {ex.Message} -> {ex.InnerException?.Message}");
                return Result<ExchangeRateDto>.Failure("Ocurrió un error inesperado al procesar y asentar el tipo de cambio.");
            }
        }

        public async Task<Result<ExchangeRateDto>> UpdateAsync(int id, ExchangeRateDto dto)
        {
            var exchange = await _uow.Appointments.FindAsync(id);

            if (exchange == null)
                return Result<ExchangeRateDto>.Failure($"El tipo de cambio con ID {id} no fue encontrada.");

            var validation = await ValidateDependenciesAsync(dto);
            if (!validation.IsSuccess) return Result<ExchangeRateDto>.Failure(validation.ErrorMessage);

            _mapper.Map(dto, exchange);
            exchange.Id = id;

            await _uow.BeginTransactionAsync();
            try
            {
                await InactivateActiveRatesAsync(dto);
                await _uow.Appointments.UpdateAsync(exchange);
                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return Result<ExchangeRateDto>.Success(_mapper.Map<ExchangeRateDto>(exchange));
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                // Loguear internamente el error real para soporte técnico antes de enmascarar la respuesta
                Console.WriteLine($"[ClinicalSuiteNova Error]: {ex.Message} -> {ex.InnerException?.Message}");
                return Result<ExchangeRateDto>.Failure("Ocurrió un error inesperado al procesar y asentar el tipo de cambio.");
            }
        }

        private async Task<Result<bool>> ValidateDependenciesAsync(ExchangeRateDto dto)
        {
            if (!await _uow.Currencies.ExistsAsync(dto.FromCurrencyId))
                return Result<bool>.Failure("FromCurrencyId no es válido.");

            if (!await _uow.Currencies.ExistsAsync(dto.ToCurrencyId))
                return Result<bool>.Failure("ToCurrencyId no es válido.");

            return Result<bool>.Success(true);
        }

        private async Task InactivateActiveRatesAsync(ExchangeRateDto dto)
        {
            if (dto.IsActive)
            {
                await _uow.ExchangeRates.InactivateActiveRatesAsync(dto.FromCurrencyId, dto.ToCurrencyId);
            }
        }
    }
}
