using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IPaymentService: IBaseService<Payment>
    {
        public Task<Result<Payment>> CreatePaymentAsync(Payment payment);
    }
}
