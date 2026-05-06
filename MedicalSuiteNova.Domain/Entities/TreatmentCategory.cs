
namespace MedicalSuiteNova.Domain.Entities
{
    public class TreatmentCategory
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
