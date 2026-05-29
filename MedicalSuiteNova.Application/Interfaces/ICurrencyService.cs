
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface ICurrencyService: IBaseService<Currency>
    {
        Task<Result<CurrencyDto>> UpdateAsync(int id, AppointmentDto dto);
    }
}
