
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class CurrencyService:BaseService<Currency>, ICurrencyService
    {
        public CurrencyService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Currencies) { }
    }
}
