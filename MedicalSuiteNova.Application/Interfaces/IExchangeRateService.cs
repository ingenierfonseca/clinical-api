
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IExchangeRateService:IBaseService<ExchangeRate>
    {
        Task<Result<ExchangeRate>> GetLatestRate(int from, int to);
        Task<Result<ExchangeRateDto>> AddAsync(ExchangeRateDto dto);
    }
}
