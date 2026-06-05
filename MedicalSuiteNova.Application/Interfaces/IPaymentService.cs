using MedicalSuiteNova.Domain.Dto.Payment;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IPaymentService: IBaseService<Payment>
    {
        public Task<Result<PaymentDto>> CreatePaymentAsync(PaymentRequest request);
        Task<Result<PaymentBaucherDto>> GetBaucher(int id);
    }
}
