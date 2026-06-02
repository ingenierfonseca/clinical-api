
using AutoMapper;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Mappings
{
    public class CurrencyProfile : Profile
    {
        public CurrencyProfile()
        {
            CreateMap<CurrencyDto, Currency>();
            CreateMap<Currency, CurrencyDto>();
            CreateMap<ExchangeRateDto, ExchangeRate>();
            CreateMap<ExchangeRate, ExchangeRateDto>();
        }
    }
}
