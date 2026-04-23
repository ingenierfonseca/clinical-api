
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class ExchangeRateService:BaseService<ExchangeRate>, IExchangeRateService
    {
        public ExchangeRateService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.ExchangeRates) { }

        public async Task<Result<ExchangeRate>> GetLatestRate(int from, int to)
        {
            var result = await _uow.ExchangeRates.FirstOrDefaultAsync(x => x.FromCurrencyId == from && x.ToCurrencyId == to && x.IsActive);

            if (result == null)
                return Result<ExchangeRate>.Failure("No se encontro el tipo de cambio para los datos ingresados");

            else return Result<ExchangeRate>.Success(result);
        }
    }
}
