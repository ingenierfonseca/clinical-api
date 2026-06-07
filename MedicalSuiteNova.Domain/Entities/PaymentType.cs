

namespace MedicalSuiteNova.Domain.Entities
{
    public class PaymentType
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
