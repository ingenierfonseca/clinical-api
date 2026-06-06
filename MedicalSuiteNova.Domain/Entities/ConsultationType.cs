using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ConsultationType: IEntity
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public object GetId() => Id;
    }
}
