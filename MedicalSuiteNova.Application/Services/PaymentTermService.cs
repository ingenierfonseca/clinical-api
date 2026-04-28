
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class PaymentTermService:BaseService<PaymentTerm>, IPaymentTermService
    {
        public PaymentTermService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.PaymentTerms) { }

        public async Task<Result<PaymentTermDto>> CreateAsync(PaymentTermDto dto)
        {
            var exist = await _uow.PaymentTerms.AnyAsync(x => x.Name == dto.Name);
            if (exist)
            {
                return Result<PaymentTermDto>.Failure("Ya existe termino de pago con ese nombre");
            }

            var paymentTerm = _mapper.Map<PaymentTerm>(dto);
            await _uow.PaymentTerms.AddAsync(paymentTerm);
            await _uow.CompleteAsync();
            return Result<PaymentTermDto>.Success(_mapper.Map<PaymentTermDto>(paymentTerm));
        }
    }
}
