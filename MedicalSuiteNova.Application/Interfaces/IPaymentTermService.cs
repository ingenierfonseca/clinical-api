
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IPaymentTermService: IBaseService<PaymentTerm>
    {
        public Task<Result<PaymentTermDto>> CreateAsync(PaymentTermDto dto);
    }
}
