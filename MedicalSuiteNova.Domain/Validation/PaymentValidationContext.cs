
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Validation
{
    public class PaymentValidationContext
    {
        public required Invoice Invoice { get; set; }
        public required Customer Customer { get; set; }
        public required decimal CurrentBalance { get; set; }
    }
}
