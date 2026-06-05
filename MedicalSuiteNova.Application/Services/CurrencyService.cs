
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class CurrencyService(IUnitOfWork uow, IMapper mapper) : BaseService<Currency>(uow, mapper, uow.Currencies), ICurrencyService
    {
        public async Task<Result<CurrencyDto>> UpdateAsync(int id, AppointmentDto dto)
        {
            var currency = await _uow.Appointments.FindAsync(id);

            if (currency == null)
                return Result<CurrencyDto>.Failure($"La moneda con ID {id} no fue encontrada.");

            _mapper.Map(dto, currency);
            currency.Id = id;

            await _uow.Appointments.UpdateAsync(currency);
            await _uow.CompleteAsync();

            return Result<CurrencyDto>.Success(_mapper.Map<CurrencyDto>(currency));
        }
    }
}
