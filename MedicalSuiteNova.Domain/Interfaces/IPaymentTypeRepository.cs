
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IPaymentTypeRepository : IBaseRepository<PaymentType>
    {
        public Task<bool> IsValidPaymentTypeAsync(int paymentTypeId);
    }
}
