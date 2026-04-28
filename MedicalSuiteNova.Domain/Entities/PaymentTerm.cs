
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class PaymentTerm : IEntity
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int DaysToDue { get; set; }
        public object GetId() => Id;
    }
}
