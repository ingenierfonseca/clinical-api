
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class CurrencyService(IUnitOfWork uow, IMapper mapper) : BaseService<Currency>(uow, mapper, uow.Currencies), ICurrencyService
    {
    }
}
