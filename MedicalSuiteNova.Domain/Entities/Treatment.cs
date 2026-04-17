
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Treatment: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte CurrencyId { get; set; }
        public decimal Price { get; set; }
        public int DurationMinutes { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public object GetId() => Id;
        public virtual Currency Currency { get; set; }
    }
}
