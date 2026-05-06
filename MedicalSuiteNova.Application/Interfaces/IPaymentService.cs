using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IPaymentService: IBaseService<Payment>
    {
        public Task<Result<PaymentDto>> CreatePaymentAsync(PaymentDto payment);
    }
}
