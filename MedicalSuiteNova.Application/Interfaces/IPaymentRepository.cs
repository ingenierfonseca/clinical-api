using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        public Task<Result<ResponsePaymentDto>> CreatePaymentAsync(Payment payment);
    }
}
